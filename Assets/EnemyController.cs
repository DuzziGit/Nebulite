using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float health = 10f;
    public float damage = 10f;
    public float moveSpeed = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    public float KBForce =2f;
    public GameObject player;
    private bool isShaking = false; // flag to indicate if the material is currently shaking
    private bool canAttack = true; // flag to indicate if the enemy can attack
    private bool isMoving = true; // flag to indicate if the enemy is moving
    public EnemySpawner EnemySpawner;
    private Vector3 originalPosition;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Assumes the player object has a "Player" tag
        originalPosition = transform.position;
    }

    private void Update()
    {
        if(isMoving) 
            MoveTowardsPlayer();
        CheckAttackPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = player.transform.position - this.transform.position;
        direction.Normalize();
        this.transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void CheckAttackPlayer()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) <= attackRange && canAttack)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    private IEnumerator AttackPlayer()
{
    canAttack = false;
    isMoving = false;

    PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
    playerMovement.TakeDamage(damage, gameObject); // Pass the enemy game object as the second argument
    playerMovement.Knockback(transform.position, KBForce);
    playerMovement.time = playerMovement.time - 3;
    yield return new WaitForSeconds(attackCooldown);

    canAttack = true;
    isMoving = true;
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
