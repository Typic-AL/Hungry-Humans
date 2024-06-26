using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour
{
    public int size = 0;
    public int level = 0;
    public float scaleFactor;

    [SerializeField] private Camera cam;
    private float camY;
    private float camZ;
    
    
    // Start is called before the first frame update
    void Start()
    {
        camY = cam.transform.position.y;
        camZ = 6.9f;
    }

    // Update is called once per frame
    void Update()
    {
        if(level == 0)
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
        Vector3 ts = transform.localScale;
        if (size >= 10*(level + 1)*.75)
        {
            transform.localScale = new Vector3(scaleFactor * ts.x, scaleFactor * ts.y, scaleFactor * ts.z);
            level += 1;
            size = 0;
            camY *= scaleFactor;
            camZ *= scaleFactor;
        }
    }
}
