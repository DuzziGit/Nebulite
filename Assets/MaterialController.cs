using UnityEngine;

public class MaterialController : MonoBehaviour
{
    public int health = 10; // initial health
    public string materialKind;
    public int materialDropMin = 1; // minimum number of materials that can be dropped
    public int materialDropMax = 5; // maximum number of materials that can be dropped
    public GameObject materialDropPrefab; // prefab for the dropped materials

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            int amountToDrop = Random.Range(materialDropMin, materialDropMax + 1);
            Debug.Log("Dropped " + amountToDrop + " of " + materialKind);

            // spawn the dropped materials
            for (int i = 0; i < amountToDrop; i++)
            {
                Instantiate(materialDropPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); // destroy the material
        }
    }
}
