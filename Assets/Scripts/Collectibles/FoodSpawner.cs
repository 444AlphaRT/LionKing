using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour
{
    // Prefab for the common food (chicken)
    [SerializeField] private GameObject commonFoodPrefab;

    // Prefab for the rare food (steak)
    [SerializeField] private GameObject rareFoodPrefab;

    // Corners of the rectangular spawn area in world space
    [SerializeField] private Transform minSpawnPoint;
    [SerializeField] private Transform maxSpawnPoint;

    // How many food items to create in each spawn wave
    [SerializeField] private int foodItemsPerSpawn = 3;

    // Chance that a spawned food item will be rare (value between 0 and 1)
    [SerializeField] private float rareFoodChance = 0.05f;

    // Minimum and maximum time between spawn waves
    [SerializeField] private float minSpawnIntervalSeconds = 3f;
    [SerializeField] private float maxSpawnIntervalSeconds = 7f;

    private void Start()
    {
        // Start spawning food items over time
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnIntervalSeconds, maxSpawnIntervalSeconds);
            yield return new WaitForSeconds(waitTime);

            SpawnFoodWave();
        }
    }

    private void SpawnFoodWave()
    {
        for (int i = 0; i < foodItemsPerSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject prefabToSpawn = ChooseFoodPrefab();

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float minX = minSpawnPoint.position.x;
        float maxX = maxSpawnPoint.position.x;
        float minY = minSpawnPoint.position.y;
        float maxY = maxSpawnPoint.position.y;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector3(randomX, randomY, 0f);
    }

    private GameObject ChooseFoodPrefab()
    {
        bool shouldSpawnRare = Random.value < rareFoodChance;
        return shouldSpawnRare ? rareFoodPrefab : commonFoodPrefab;
    }
}