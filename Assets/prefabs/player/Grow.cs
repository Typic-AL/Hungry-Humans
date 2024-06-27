using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour
{
    public int size = 0;
    public int level = 0;
    public float scaleFactor;
    public float scaleDuration;

    [SerializeField] private Camera cam;
    private float camY;
    private float camTargetY;
    private float camInitY;
    private float camZ;
    private float camTargetZ;
    private float camInitZ;

    private Vector3 scale;
    private Vector3 targetScale;
    private bool canGrow = false;
    private float timeElapsed = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        camTargetY = cam.transform.position.y;
        camY = camTargetY;
        camInitY = camY;
        camTargetZ = 6.9f;
        camZ = camTargetZ;
        camInitZ = camZ;
        
    }

    // Update is called once per frame
    void Update()
    {
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
            grow();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Eat(col.gameObject.GetComponent<FoodItem>());
        grow();
    }

    private void Eat(FoodItem f)
    {
        if(f.sizeReq <= level)
        {
            size += f.sizeRewarded;
            Destroy(f.gameObject);
        }
    }

    private void grow()
    {
        timeElapsed = 0;
        scale = transform.localScale;
        targetScale = new Vector3 (scale.x * scaleFactor, scale.y * scaleFactor, scale.z * scaleFactor);
        if (size >= 10*(level + 1)*.75)
        {
            canGrow = true;
            level += 1;
            size = 0;
            camInitY = cam.transform.position.y;
            camInitZ = camZ;
            camTargetY *= scaleFactor;
            camTargetZ *= scaleFactor;
        }
    }
}
