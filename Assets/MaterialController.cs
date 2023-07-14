using UnityEngine;
using System.Collections;

public class MaterialController : MonoBehaviour
{
    public int health = 10; // initial health
    public string materialKind;
    public int materialDropMin = 1; // minimum number of materials that can be dropped
    public int materialDropMax = 5; // maximum number of materials that can be dropped
    public float spreadRadius = 1.5f; // radius within which the materials will spread out
    public GameObject materialDropPrefab; // prefab for the dropped materials

    private bool isShaking = false; // flag to indicate if the material is currently shaking

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            int amountToDrop = Random.Range(materialDropMin, materialDropMax + 1);
            Debug.Log("Dropped " + amountToDrop + " of " + materialKind);

            // spread out the dropped materials
            for (int i = 0; i < amountToDrop; i++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * spreadRadius;
                Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);
                Instantiate(materialDropPrefab, spawnPosition, Quaternion.identity);
            }

            Destroy(gameObject); // destroy the material
        }
        else if (!isShaking) // start shaking only if the material is not already shaking
        {
            StartCoroutine(ShakeMaterial());
        }
    }

    private IEnumerator ShakeMaterial()
    {
        isShaking = true;

        float shakeDuration = 0.2f;
        float shakeIntensity = 0.1f;
        Vector3 originalPosition = transform.position;

        while (shakeDuration > 0)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeIntensity;
            shakeDuration -= Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wire sphere representing the spread radius in the scene view
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spreadRadius);
    }
}
