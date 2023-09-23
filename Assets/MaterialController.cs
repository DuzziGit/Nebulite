using UnityEngine;
using System.Collections;


public class MaterialController : MonoBehaviour
{
    public int health = 10;
    public string materialKind;
    public int materialDropMin = 1;
    public int materialDropMax = 5;
    public float spreadRadius = 1.5f;
    public GameObject materialDropPrefab;
    public AudioClip breakSound; // sound to play when the material breaks
    public AudioClip damageSound; // sound to play when the material takes damage

    private bool isShaking = false;
    private AudioSource audioSource; // audio source to play the sound

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // add an AudioSource component to the game object
    }

  public void TakeDamage(int amount)
{
    health -= amount;

    if (health <= 0)
    {
        // Create a new GameObject to play the break sound
        GameObject soundObject = new GameObject("BreakSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(breakSound);

        // Destroy the soundObject after the sound has finished playing
        Destroy(soundObject, breakSound.length);

        int amountToDrop = Random.Range(materialDropMin, materialDropMax + 1);
        Debug.Log("Dropped " + amountToDrop + " of " + materialKind);

        for (int i = 0; i < amountToDrop; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spreadRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);
            Instantiate(materialDropPrefab, spawnPosition, Quaternion.identity);
        }

        // Destroy the original object immediately
        Destroy(gameObject);
    }
    else
    {
        PlaySound(damageSound); // play damage sound

        if (!isShaking)
        {
            StartCoroutine(ShakeMaterial());
        }
    }
}



    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // play the assigned sound clip
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spreadRadius);
    }
}
