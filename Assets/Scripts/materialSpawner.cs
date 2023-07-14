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
    public float innerCircleRadius = 2f;
    public LayerMask platformLayer;

    private List<GameObject> platforms = new List<GameObject>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0), circleRadius);

        Gizmos.color = Color.red;
        float noSpawnRadius = innerCircleRadius; 
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0), noSpawnRadius);
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
            float noSpawnRadius = innerCircleRadius;
            int attempts = 100;  // limit the number of attempts to find a suitable position

            // Try to find a suitable position to spawn the prefab.
            // Repeat this process until a valid position is found.
            do
            {
                // generate a random angle and a random radius between the noSpawnRadius and circleRadius
                float angle = Random.Range(0, 360);
                float radius = Random.Range(noSpawnRadius, circleRadius);

                // calculate the position based on the angle and radius
                position = new Vector2(
                    transform.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad),
                    transform.position.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad)
                );

                hitCollider = Physics2D.OverlapBox(position, size, 0, platformLayer);
            } while (hitCollider != null && --attempts > 0); // decrement attempts each time the loop runs

            if (attempts > 0)  // if a suitable position was found
            {
                GameObject platform = Instantiate(prefab, position, Quaternion.identity);
                platforms.Add(platform);
            }
        }
    }


}


