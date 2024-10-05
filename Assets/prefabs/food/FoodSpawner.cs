using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GridManager gridManager;
    
    public List<FoodItem> foodPrefabs;
    public Grow player;
    public int totalFoodCount;
    public int quickSpawnCount;
    public float spawnIntervalMin = 1f;
    public float spawnIntervalMax = 5f;
    [HideInInspector] public float xMin = -59f;
    [HideInInspector] public float xMax = 59f;
    [HideInInspector] public float yMin = -59f;
    [HideInInspector] public float yMax = 59;
    public float spacingMargin = 30f;

    private int numFoodSpawned = 0;

    public List<GameObject> spawnedFood = new List<GameObject>();

    void Awake()
    {
        gm.i.foodSpawner = GetComponent<FoodSpawner>();
    }
    
    void Start()
    {
        StartCoroutine(SpawnFoodRoutine());
    }

    private void Update()
    {
    
    }

    IEnumerator SpawnFoodRoutine()
    {
        while (true)
        {
            if (spawnedFood.Count < totalFoodCount)
            {
                SpawnFood();
                
            }
            if(spawnedFood.Count <= quickSpawnCount)
                yield return new WaitForSeconds(.001f);
            else
                yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
        }
    }

    void SpawnFood()
    {
        Vector2 randomPosition = GetRandomPosition();
        GameObject foodToSpawn = GetRandomFoodPrefab();
        GameObject spawned = Instantiate(foodToSpawn, new Vector3(randomPosition.x, foodToSpawn.transform.position.y, randomPosition.y), Quaternion.identity);
        spawnedFood.Add(spawned);
    }

    Vector2 GetRandomPosition()
    {
        float x = Random.Range(xMin, xMax);
        float y = Random.Range(yMin, yMax);
        return new Vector2(x, y);
    }

    GameObject GetRandomFoodPrefab()
    {
        float totalProbability = 0f;
        foreach (var foodPrefab in foodPrefabs)
        {
            totalProbability += foodPrefab.spawnProbability;
        }

        float randomPoint = Random.value * totalProbability;
        foreach (var foodPrefab in foodPrefabs)
        {
            if (randomPoint < foodPrefab.spawnProbability)
            {
                return foodPrefab.gameObject;
            }
            else
            {
                randomPoint -= foodPrefab.spawnProbability;
            }
        }
        return foodPrefabs[0].gameObject;
    }
    
}
