using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grow : MonoBehaviour
{
    public int size = 0;
    public int level = 5;
    public float scaleFactor;
    public float scaleDuration;

    [SerializeField] private AudioSource chompAudio;
    [SerializeField] private AudioSource inflateAudio;

    [SerializeField] private Camera cam;
    [SerializeField] private FoodSpawner foodSpawner;

    #region camera pos variables
    private float camY;
    private float camTargetY;
    private float camInitY;
    private float camZ;
    private float camTargetZ;
    private float camInitZ;
    #endregion

    [SerializeField] public Timer foodTimer;

    #region scale variables
    private Vector3 scale;
    private Vector3 targetScale;
    private bool canGrow = false;
    private float timeElapsed = 0;
    #endregion

    [SerializeField] private TextMeshProUGUI levelText;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        gm.i.player = GetComponent<Grow>();
    }
    
    void Start()
    {
        
        camTargetY = cam.transform.position.y;
        camY = camTargetY;
        camInitY = camY;
        camTargetZ = 7.12f;
        camZ = camTargetZ;
        camInitZ = camZ;

        levelText.text = "Level: " + (level + 1).ToString();
        gm.i.foodTimerCountdownAmount = Time.deltaTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.i.gc.gameOver)
        {
            foodTimer.StopTimer();
        }
        gm.i.foodTimerCountdownAmount = Time.deltaTime + (Time.deltaTime * .1f * level * 2);
        if(canGrow)
        {
            if (timeElapsed <= scaleDuration)
            {
                timeElapsed += Time.deltaTime;
                float progress = timeElapsed / scaleDuration;
                transform.localScale = Vector3.Lerp(scale, targetScale, progress);
                camY = Mathf.Lerp(camInitY, camTargetY, progress);
                camZ = Mathf.Lerp(camInitZ, camTargetZ, progress);
            }
            else
                canGrow = false;
        }

        if (level == 0)
            cam.transform.position = new Vector3(gameObject.transform.position.x, cam.transform.position.y, gameObject.transform.position.z - 6.9f);
        else
            cam.transform.position = new Vector3(gameObject.transform.position.x, camY, gameObject.transform.position.z - camZ);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.GetComponent<FoodItem>() != null)
        {
            Eat(col.gameObject.GetComponent<FoodItem>());
            
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Eat(col.gameObject.GetComponent<FoodItem>());
        
    }

    private void Eat(FoodItem f)
    {
        if(f.sizeReq <= level && !gm.i.gc.gameOver && checkAILvl(f))
        {
            
            
            chompAudio.PlayOneShot(chompAudio.clip);
            foodTimer.seconds = 10;//+ (level);
            foodTimer.timeRemaining += f.timeAdded;
            if (foodTimer.timeRemaining >= foodTimer.seconds)
                foodTimer.timeRemaining = foodTimer.seconds;
            //foodTimer.StartTimer();
            size += (int)(f.sizeRewarded * (1 + (gm.i.upgrades.range.lvl / 10)));

            foodSpawner.spawnedFood.Remove(f.gameObject);
            
            if(f.gameObject.gameObject.GetComponent<ai>() != null)
                gm.i.spawnedAis.Remove(f.gameObject.gameObject.GetComponent<ai>());
            
            Destroy(f.gameObject);
            grow();
        }
    }

    private bool checkAILvl(FoodItem f)
    {
        ai _ai = null;
            
            if(f.gameObject.gameObject.GetComponent<ai>() != null)
                _ai = f.gameObject.GetComponent<ai>();
            else
                return true;

            if(_ai != null && level > _ai.level)
                return true;
        
        return false;
    }

    /*public void TestPrint()
    {
        print("oh my goodness");
    }*/

    private void grow()
    {

        if (size >= 10*(level + 1)*.75)
        {
            timeElapsed = 0;
            scale = transform.localScale;
            targetScale = new Vector3(scale.x * scaleFactor + .2f, scale.y * scaleFactor - .1f, scale.z * scaleFactor + .1f);

            level += 1;
            levelText.text = "Level: " + (level + 1).ToString();


            size = 0;
            camInitY = cam.transform.position.y;
            camInitZ = camZ;
            camTargetY *= scaleFactor - .05f;
            camTargetZ *= scaleFactor - .05f;
            canGrow = true;
            inflateAudio.PlayOneShot(inflateAudio.clip);
        }
    }

    public void ResetSize()
    {
        // timeElapsed = 0;
        // scale = transform.localScale;
        // targetScale = new Vector3(scale.x * scaleFactor + .2f, scale.y * scaleFactor - .1f, scale.z * scaleFactor + .1f);

        // level = 0;
        // levelText.text = "Level: " + (level + 1).ToString();


        // size = 0;
        // camInitY = cam.transform.position.y;
        // camInitZ = camZ;
        // camTargetY = camResetY;
        // camTargetZ = camResetZ;
        // canGrow = true;

        gameObject.SetActive(false);
    }
}
