using System.Collections.Generic;
using UnityEngine;

public class materialSpawner : MonoBehaviour
{
    public int maxPlatforms = 10;
    public GameObject[] platformPrefabs;
    public float xPadding = 1;
    public float yPadding = 1;
    public Vector2 minSpawnPosition = new Vector2(0, 0);
    public Vector2 maxSpawnPosition = new Vector2(35, 40);
    public float circleRadius = 50f;
    public LayerMask platformLayer;

    private List<GameObject> platforms = new List<GameObject>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 spawnAreaSize = new Vector3(maxSpawnPosition.x - minSpawnPosition.x, maxSpawnPosition.y - minSpawnPosition.y, 0);
        Vector3 spawnAreaCenter = new Vector3(minSpawnPosition.x + spawnAreaSize.x / 2, minSpawnPosition.y + spawnAreaSize.y / 2, 0);
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0), circleRadius);
    }

    private void Start()
    {
        for (int i = 0; i < maxPlatforms; i++)
        {
            GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

            Vector2 size = prefab.GetComponent<BoxCollider2D>().bounds.size;
            size += new Vector2(xPadding * 2, yPadding * 2);

            Vector2 position;
            Collider2D hitCollider;

            // Try to find a suitable position to spawn the prefab.
            // Repeat this process until a valid position is found.
            do
            {
                position = new Vector2(
                    Random.Range(minSpawnPosition.x + size.x / 2, maxSpawnPosition.x - size.x / 2),
                    Random.Range(minSpawnPosition.y + size.y / 2, maxSpawnPosition.y - size.y / 2)
                );
                hitCollider = Physics2D.OverlapBox(position, size, 0, platformLayer);
            } while (hitCollider != null);

            GameObject platform = Instantiate(prefab, position, Quaternion.identity);
            platforms.Add(platform);
        }
    }
}


