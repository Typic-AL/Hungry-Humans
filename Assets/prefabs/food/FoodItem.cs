using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FoodItem : MonoBehaviour
{
    public enum Rarity
    {
        common,
        rare,
        epic,
        legendary
    }

    public int sizeReq;
    public int sizeRewarded;
    public float spawnProbability;

    public Rarity rarity;
    public float spawnRate;
    public float timeAdded;

    // Start is called before the first frame update
    void Start()
    {
        switch(rarity)
        {
            case Rarity.common:
                spawnRate = 50f;
                timeAdded = 1f;
                break;
            case Rarity.rare:
                spawnRate = 25f;
                timeAdded = 2f;
                break;
            case Rarity.epic:
                spawnRate = 15f;
                timeAdded = 3f;
                break;
            case Rarity.legendary:
                spawnRate = 7f;
                timeAdded = 4f;
                break;


        }

        spawnProbability = spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetSpawnRate()
    {
        return spawnRate;
    }

    public float GetTimeAdded()
    {
        return timeAdded;
    }
}
