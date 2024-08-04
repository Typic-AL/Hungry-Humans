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
    [HideInInspector] public int sizeRewarded;
    [HideInInspector] public float spawnProbability;

    public Rarity rarity;
    [HideInInspector] public float spawnRate;
    [HideInInspector] public float timeAdded;

    // Start is called before the first frame update
    void OnValidate()
    {
        switch(rarity)
        {
            case Rarity.common:
                spawnRate = 35f;
                timeAdded = 1f;
                sizeRewarded = 1;
                break;
            case Rarity.rare:
                spawnRate = 25f;
                timeAdded = 2f;
                sizeRewarded = 2;
                break;
            case Rarity.epic:
                spawnRate = 15f;
                timeAdded = 3f;
                sizeRewarded = 3 * (sizeReq + 1);
                break;
            case Rarity.legendary:
                spawnRate = 5f;
                timeAdded = 4f;
                sizeRewarded = 4 * (sizeReq + 1);
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
