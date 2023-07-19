using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int maxEnemies = 10;
    public GameObject[] enemyPrefabs;
    public float spawnRate = 1f; // enemies per second
    public Vector2 minSpawnPosition = new Vector2(0, 0);
    public Vector2 maxSpawnPosition = new Vector2(35, 40);
    public float spawnRadius = 50f;
    public float noSpawnRadius = 2f;

    private List<GameObject> enemies = new List<GameObject>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0), spawnRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0), noSpawnRadius);
    }

    private void Start()
    {
        // Start spawning enemies periodically
        InvokeRepeating(nameof(SpawnEnemy), 0f, 1f/spawnRate);
    }

    private void SpawnEnemy()
    {
        if (enemies.Count >= maxEnemies)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // generate a random angle and a random radius between the noSpawnRadius and spawnRadius
        float angle = Random.Range(0, 360);
        float radius = Random.Range(noSpawnRadius, spawnRadius);

        // calculate the position based on the angle and radius
        Vector2 position = new Vector2(
            transform.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad),
            transform.position.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        if(enemies.Count < maxEnemies && !IsInvoking(nameof(SpawnEnemy)))
        {
            InvokeRepeating(nameof(SpawnEnemy), 0f, 1f/spawnRate);
        }
    }
}
