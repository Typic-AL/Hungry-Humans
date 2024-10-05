using System.Runtime;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using System;


public class ai : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private FoodSpawner foodSpawner;
    private Vector3 lastPosition;
    public int size;
    public int level;

    private FoodItem FoodComponent;

    private Rigidbody rb;

    private bool canGrow = false;
    public float scaleDuration;
    private Vector3 scale;
    private Vector3 targetScale;
    private float timeElapsed = 0;

    private Transform playerTransform = null;
    private Grow player = null;
    private Transform foodTransform;
    private Transform aiTransform;
    private ai targetedAi;
    private float moveSpeed = 4.5f;
    private float rotationSpeed = 10f;

    public bool targetAquired = false;
    public string target;

    Vector3 movementDirection = new Vector3(0, 0, 0);
    float distanceToMove = 0;
    Vector3 chosenTargetPos;

    

    private int arrayPos;

    // Start is called before the first frame update
    
    void Start()
    {
        
        player = gm.i.player;
        FoodComponent = GetComponent<FoodItem>();
        FoodComponent.sizeReq = level;
        
        foodSpawner = gm.i.foodSpawner;
        gm.i.spawnedAis.Add(GetComponent<ai>());
        
        arrayPos = gm.i.spawnedAis.IndexOf(GetComponent<ai>());

        rb = GetComponent<Rigidbody>();

        moveSpeed *= (1 + ((float)gm.i.upgrades.speed.lvl - 1) / 10);

        
    }

    // Update is called once per frame
    
    void Update()
    {
        playerTransform = player.gameObject.transform;

        
        //print(target + " " + arrayPos);
        
        
        if (animator != null && Time.deltaTime > 0.0f)
        {
            float distanceTraveledSinceLastFrame = (transform.position - lastPosition).magnitude;
            float distancePerSecond = distanceTraveledSinceLastFrame / Time.deltaTime;

            animator.SetFloat("Speed", distancePerSecond);
        }
        

        lastPosition = transform.position;

        if(canGrow)
        {
            if (timeElapsed <= scaleDuration)
            {
                timeElapsed += Time.deltaTime;
                float progress = timeElapsed / scaleDuration;
                transform.localScale = Vector3.Lerp(scale, targetScale, progress);
                
            }
            else
                canGrow = false;
        }

        
    }

    void UpdateTarget()
    {
        int detectionRange = 50;

        LayerMask mask = LayerMask.GetMask("Player and AI");

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, mask);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.name.Contains("player") && level > player.level)
            {
                target = "player";
                //break;
            }
            else if(hit.gameObject.name.Contains("ai") && level > hit.gameObject.GetComponent<ai>().level && hit.gameObject != gameObject)
            {   
                target = "ai";
                targetedAi = hit.gameObject.GetComponent<ai>();
                //break;
            }
            
        }
        
        
        if(target == "player" && player.gameObject.activeSelf)
            chosenTargetPos = playerTransform.position;
        else if(target == "ai" && targetedAi != null && level > targetedAi.level)
            chosenTargetPos = targetedAi.transform.position;
        else
        {
            target = "food";

            if(foodTransform == null) foodTransform = FindClosestFood(transform.position);

            chosenTargetPos = foodTransform.position;
        }
    
    }
    
    void FixedUpdate()
    {
        UpdateTarget();

        // Calculate the direction to the target
        Vector3 direction = (chosenTargetPos - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, chosenTargetPos);
        
        rb.velocity = Vector3.Lerp(rb.velocity, direction * moveSpeed, Time.deltaTime * 2f);
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    

    public Transform FindClosestFood(Vector3 position)
    {
        int searchRadius = 50;
        Collider[] nearbyObjects = Physics.OverlapSphere(position, searchRadius);
        GameObject closestFood = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider collider in nearbyObjects)
        {
            GameObject food = collider.gameObject;
            if(!CheckFoodList(food)) continue;
            if(gm.i.aiTargetedFood.Contains(food) && !gm.i.aiTargetedFood[arrayPos] == food) continue;
            
            
            float distance = Vector3.SqrMagnitude(position - food.transform.position);
            if (distance < closestDistance)
            {
                closestFood = food;
                closestDistance = distance;
            }
        }

        gm.i.aiTargetedFood[arrayPos] = closestFood;

        return closestFood.transform;
        
    }

    private bool CheckFoodList(GameObject ob)
    {
        if(ob.GetComponent<FoodItem>())
        {
            FoodItem item = ob.GetComponent<FoodItem>();
            if(!item.playerOnly)
                return true;
        }

        return false;
    }

    

    //checks for closest target out of the 3
    private bool checkFoodNull()
    {
        if(foodTransform == null)
        {
            print("food null");
            return false;
        }
        // if (targetedAi == null)
        // {
        //     print("ai null");
        //     return false;
        // }
        if (playerTransform == null)
        {
            print("player null");
            return false;
        }
        return true;
    }

    private void RemoveTargetedFood(GameObject foodItem)
    {
        //foodSpawner.spawnedFood.Remove(foodItem);
        gm.i.aiTargetedFood[arrayPos] = null;//new GameObject();
    }


    // Check if the enemy is bigger than the player and if so, attack the player
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player.gameObject)
        {
            // float playerSize = player.transform.localScale.x;
            // float enemySize = transform.localScale.x;
            if (level > player.level)
            {
                player.ResetSize();
                targetAquired = false;
            }
        }
        else if (other.gameObject.GetComponent<ai>())
        {
            ai _ai = other.gameObject.GetComponent<ai>();
            if (level > _ai.level)
            {
                foodSpawner.spawnedFood.Remove(gm.i.aiTargetedFood[_ai.arrayPos]);
                gm.i.aiTargetedFood[_ai.arrayPos] = null;//new GameObject();
                gm.i.spawnedAis.Remove(targetedAi);
                Destroy(targetedAi.gameObject);
                targetAquired = false;
            }
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<FoodItem>())
            Eat(other.gameObject.GetComponent<FoodItem>());
    }

    

    

    private void Eat(FoodItem f)
    {
        if(f.sizeReq <= level)
        {
            size += (int)(f.sizeRewarded * (1 + ((float)(gm.i.upgrades.range.lvl - 1) / 10)));
            RemoveTargetedFood(f.gameObject);
            foodSpawner.spawnedFood.Remove(f.gameObject);
            Destroy(f.gameObject);
            grow();
            if(!f.playerOnly) targetAquired = false;
        }
    }

    private void grow()
    {
        
        if (size >= 10*(level + 1)*.75)
        {
            timeElapsed = 0;
            scale = transform.localScale;
            targetScale = new Vector3(scale.x * 1.3f + .2f, scale.y * 1.3f - .1f, scale.z * 1.3f + .1f);

            level += 1;
            size = 0;
            FoodComponent.sizeReq = level;
            canGrow = true;
        }
    }

    

    
}


