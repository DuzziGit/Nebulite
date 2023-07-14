using UnityEngine;

public class materialSpawner : MonoBehaviour
{
    public GameObject materialPrefab;
    public int numberOfMaterials;
    public Vector2 minSpawnBoundary;
    public Vector2 maxSpawnBoundary;
    public float spawnRadius;

    void Start()
    {
        SpawnMaterials();
    }

    void SpawnMaterials()
    {
        for (int i = 0; i < numberOfMaterials; i++)
        {
            float x = Random.Range(minSpawnBoundary.x, maxSpawnBoundary.x);
            float y = Random.Range(minSpawnBoundary.y, maxSpawnBoundary.y);

            // Apply a random offset within the spawn radius
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(x + randomOffset.x, y + randomOffset.y, 0);

            Instantiate(materialPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
