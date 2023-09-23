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
        public AudioClip deathSound; // sound to play when the enemy dies
            public AudioClip attackSound; // sound to play when the enemy attacks


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
        PlayAttackSound(); // play attack sound

    PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
    playerMovement.TakeDamage(damage, gameObject); // Pass the enemy game object as the second argument
    playerMovement.Knockback(transform.position, KBForce);
    playerMovement.time = playerMovement.time - 14;
    yield return new WaitForSeconds(attackCooldown);

    canAttack = true;
    isMoving = true;
}


    public void TakeDamage(float amount)
    {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

      //  Debug.Log("Taking damage: " + amount);
        health -= amount;
        if (health <= 0)
        {
          //  Debug.Log("Enemy destroyed");
            playerMovement.coins += 1;
            playerMovement.AddMaterial("Enemy");
            EnemySpawner.RemoveEnemy(this.gameObject);
            Destroy(this.gameObject);
                        PlayDeathSound(); // play death sound


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
      private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            // Create a new GameObject to play the death sound
            GameObject soundObject = new GameObject("DeathSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(deathSound);

            // Destroy the soundObject after the sound has finished playing
            Destroy(soundObject, deathSound.length);
        }


    }
        private void PlayAttackSound()
    {
        if (attackSound != null)
        {
            // Create a new GameObject to play the attack sound
            GameObject soundObject = new GameObject("AttackSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(attackSound);

            // Destroy the soundObject after the sound has finished playing
            Destroy(soundObject, attackSound.length);
        }
    } 
}
