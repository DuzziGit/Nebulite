using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float health = 10f;
    public float damage = 10f;
    public float moveSpeed = 5f;
    public GameObject player;
    private bool isShaking = false; // flag to indicate if the material is currently shaking
    public EnemySpawner EnemySpawner;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Assumes the player object has a "Player" tag
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = player.transform.position - this.transform.position;
        direction.Normalize();

        this.transform.position += direction * moveSpeed * Time.deltaTime;
    }

  public void TakeDamage(float amount)
{
    Debug.Log("Taking damage: " + amount);
    health -= amount;
    if (health <= 0)
    {
        Debug.Log("Enemy destroyed");
        EnemySpawner.RemoveEnemy(this.gameObject);
        Destroy(this.gameObject);
    }
    else if (!isShaking)
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

}
