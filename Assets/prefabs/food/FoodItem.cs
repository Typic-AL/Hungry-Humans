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

    public bool playerOnly;

    public Rarity rarity;
    [HideInInspector] public float spawnRate;
    [HideInInspector] public int timeAdded;

    public float heightOffset;

    // Start is called before the first frame update
    void OnValidate()
    {
        switch(rarity)
        {
            case Rarity.common:
                spawnRate = 35f;
                timeAdded = 1;
                sizeRewarded = 1;
                break;
            case Rarity.rare:
                spawnRate = 25f;
                timeAdded = 2;
                sizeRewarded = 2;
                break;
            case Rarity.epic:
                spawnRate = 15f;
                timeAdded = 3;
                sizeRewarded = 3 * (sizeReq + 1);
                break;
            case Rarity.legendary:
                spawnRate = 5f;
                timeAdded = 4;
                sizeRewarded = 4 * (sizeReq + 1);
                break;


        }

        spawnProbability = spawnRate;
    }

    private void Start()
    {
        if(heightOffset != 0)
            transform.position = new Vector3(transform.position.x, heightOffset, transform.position.z);
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
