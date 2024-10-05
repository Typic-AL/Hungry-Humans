using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Timer timer;
    public GameObject walls;
    private GameObject wall2;
    private GameObject wall3;
    private GameObject wall4;
    public float foodSpawningBorder = 59f;
    public FoodSpawner foodSpawner;
    private bool canShrink = false;
    private float timeElapsed = 0;
    private float timeToShrink = 3f;
    private Vector3 wall;

    private float shrinkAmount;

    Vector3 wall2Pos;
    Vector3 wall3Pos;
    Vector3 wall4Pos;
    
    // Start is called before the first frame update
    void Start()
    {
        foodSpawner.xMax = foodSpawningBorder;
        foodSpawner.xMin = -foodSpawningBorder;
        foodSpawner.yMin = -foodSpawningBorder + 5;
        foodSpawner.yMax = foodSpawningBorder;
    }

    // Update is called once per frame
    void Update()
    {
        if(canShrink)
        {
            if (timeElapsed <= timeToShrink)
            {
                timeElapsed += Time.deltaTime;
                float progress = timeElapsed / timeToShrink;
                float multi = (1 + 1/3f);
                //shrinkAmount = Mathf.Abs(wall3Pos.x - (wall3Pos.x / multi));
                
                wall2.transform.position = new Vector3(wall2Pos.x, wall2Pos.y, Mathf.Lerp(wall2Pos.z, wall2Pos.z - (shrinkAmount * 2), progress));
                wall3.transform.position = new Vector3(Mathf.Lerp(wall3Pos.x, wall3Pos.x - shrinkAmount, progress), wall3Pos.y, wall3Pos.z);
                wall4.transform.position = new Vector3(Mathf.Lerp(wall4Pos.x, wall4Pos.x + shrinkAmount, progress), wall4Pos.y, wall4Pos.z);
                
            }
            else
                canShrink = false;
                timer.StartTimer();
        }
    }

    public void ShrinkMap()
    {
        timeElapsed = 0;
        getChildren();
        canShrink = true;
        float multi = (1 + 1/3f);
        float multi2 = 1.25f;
        //foodSpawningBorder /= multi;
        wall = walls.transform.localScale;
        float tfc;
        float qsc;

        tfc = foodSpawner.totalFoodCount / multi2;
        qsc = foodSpawner.quickSpawnCount / multi2;
        foodSpawner.totalFoodCount = (int)tfc;
        foodSpawner.quickSpawnCount = (int)qsc;
        
        // foodSpawner.xMax = foodSpawningBorder;
        // foodSpawner.xMin = -foodSpawningBorder;
        // //foodSpawner.yMin = -foodSpawningBorder;
        // foodSpawner.yMax = foodSpawningBorder;
        shrinkAmount = Mathf.Abs(wall3Pos.x - (wall3Pos.x / multi));
        //foodSpawningBorder -= shrinkAmount;
        //print("border: " + foodSpawningBorder);
                
        foodSpawner.xMax -= shrinkAmount;
        foodSpawner.xMin += shrinkAmount;
        foodSpawner.yMax -= shrinkAmount * 2;

        timer.StopTimer();
    }

    private void getChildren()
    {
        for(int i = 0; i < walls.transform.childCount; i++)
        {
            GameObject wall = walls.transform.GetChild(i).gameObject;
            switch(wall.name)
            {
                case "wall2":
                    wall2 = wall;
                    wall2Pos = wall2.transform.position;
                    break;
                case "wall3":
                    wall3 = wall;
                    wall3Pos = wall3.transform.position;
                    break;
                case "wall4":
                    wall4 = wall;
                    wall4Pos = wall4.transform.position;
                    break;
            }
        }
    }
}
