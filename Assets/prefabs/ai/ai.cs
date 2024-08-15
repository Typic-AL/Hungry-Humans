using System.Runtime;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    
    void Start()
    {
        
        player = gm.i.player;
        FoodComponent = GetComponent<FoodItem>();
        FoodComponent.sizeReq = level;
        
        foodSpawner = gm.i.foodSpawner;
        gm.i.spawnedAis.Add(GetComponent<ai>());

        rb = GetComponent<Rigidbody>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        playerTransform = player.gameObject.transform;

        if(!targetAquired)
            AquireTargetFood();
        else
        {
            if(targetedAi != null)
                aiTransform = targetedAi.gameObject.transform;
            else
                target = "food";

            if(target == "player" && level > player.level)
                chosenTargetPos = playerTransform.position;
            else if(target == "ai" && level > targetedAi.level)
                chosenTargetPos = aiTransform.position;
            else
                target = "food";
                if(foodTransform == null) foodTransform = FindClosestFood();
                chosenTargetPos = foodTransform.position;
        }

        
        
        

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
    
    void FixedUpdate()
    {

        #region movement
        // Calculate the direction to the target
        Vector3 direction = (chosenTargetPos - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, chosenTargetPos);
        

        // if (distanceToTarget < turnRadius) {
        //     // Move directly towards the target
        //     rb.velocity = Vector3.Lerp(rb.velocity, direction * moveSpeed, Time.deltaTime * 5f);
        // } else {
        //     // Move in a circular motion
        //     Vector3 up = Vector3.up; // or any other vector that is perpendicular to the plane of motion
        //     Vector3 circularDirection = Vector3.Cross(direction, up).normalized;
        //     Vector3 circularVelocity = circularDirection * moveSpeed;
        //     rb.velocity = Vector3.Lerp(rb.velocity, circularVelocity, Time.deltaTime * 5f);
        // }
        rb.velocity = Vector3.Lerp(rb.velocity, direction * moveSpeed, Time.deltaTime * 2f);
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        transform.rotation = Quaternion.LookRotation(rb.velocity);
        #endregion
        
        
    }

    private Transform FindClosestFood()
    {
        Transform closestFood = null;
        float minDistance = Mathf.Infinity;

        // Iterate over all food items
        foreach (GameObject foodItem in foodSpawner.spawnedFood)
        {
            // Check if the food item is not already targeted by another AI
            FoodItem food = foodItem.GetComponent<FoodItem>();
            float distance = Vector3.Distance(transform.position, foodItem.transform.position);
            if (distance <= minDistance)
            {
                closestFood = foodItem.transform;
                minDistance = distance;
            }

                
            
        }
        return closestFood;
    }

    //checks for closest target out of the 3
    private bool checkFoodNull()
    {
        if(foodTransform == null)
        {
            print("food null");
            return false;
        }
        if (targetedAi == null)
        {
            print("ai null");
            return false;
        }
        if (playerTransform == null)
        {
            print("player null");
            return false;
        }
        return true;
    }

    private void RemoveTargetedFood(GameObject foodItem)
    {
        gm.i.aiTargetedFood.Remove(foodItem);
    }
    
    private void AquireTargetFood()
    {
        
        foodTransform = FindClosestFood();
        targetedAi = GetTargetedAI();

        if(checkFoodNull())
        {
            float distance1 = Vector3.Distance(transform.position, playerTransform.position); //distance to player
            float distance2 = Vector3.Distance(transform.position, targetedAi.transform.position); // distance to ai

            float closest = Mathf.Min(distance1, distance2);

            if (closest == distance1 && level > player.level)
                target = "player";
            else if (closest == distance2 && level > targetedAi.level)
                target = "ai";
            else
                target = "food";
            
            targetAquired = true;
        
        }
    }

    private Transform FindClosestAI()
    {
        //Transform aiTransform = null;
        float minDistance = Mathf.Infinity;
        aiTransform = null;

        // Iterate over all AIs in the scene
        foreach (ai ai in gm.i.spawnedAis)
        {
            if (ai.gameObject != gameObject) // Ignore the current AI
            {
                float distance = Vector3.Distance(transform.position, ai.transform.position);
                if (distance < minDistance)
                {
                    aiTransform = ai.transform;
                    targetedAi = ai;
                    minDistance = distance;
                }
            }
        }

        return aiTransform;
    }

    private ai GetTargetedAI()
    {
        ai target = null;
        float minDistance = Mathf.Infinity;

        foreach (ai ai in gm.i.spawnedAis)
        {
            if (ai.gameObject != gameObject) // Ignore the current AI
            {
                float distance = Vector3.Distance(transform.position, ai.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = ai;
                }
            }
        }
        return target;
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
                gm.i.spawnedAis.Remove(targetedAi);
                Destroy(targetedAi.gameObject);
                targetAquired = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Eat(other.gameObject.GetComponent<FoodItem>());
    }

    private void Eat(FoodItem f)
    {
        if(f.sizeReq <= level)
        {
            size += f.sizeRewarded;
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


