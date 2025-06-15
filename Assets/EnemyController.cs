using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{

    const string idleUp = "IdleAnimUp";
    const string idleDown = "IdleAnimDown";
    const string idleRight = "IdleAnimRight";
    const string idleLeft = "IdleAnimLeft";
    const string walkUp = "WalkUp";
    const string walkDown = "WalkDown";
    const string walkRight = "WalkSideRight";
    const string walkLeft = "WalkSideLeft";
    const string walkUpRight = "WalkUpRight";
    const string walkUpLeft = "WalkUpLeft";
    const string walkDownRight = "WalkDownRight";
    const string walkDownLeft = "WalkDownLeft";
    private Vector2 lastMoveDirection;

    private Animator animator;
    private int direction;
    public float health = 1f;
    public float damage = 2f;
    public float moveSpeed = 2f;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    public float KBForce =.2f;
    public GameObject player;
    private bool isShaking = false; // flag to indicate if the material is currently shaking
    private bool canAttack = true; // flag to indicate if the enemy can attack
    private bool isMoving = true; // flag to indicate if the enemy is moving
    public EnemySpawner EnemySpawner;
    private Vector3 originalPosition;
    public PlayerMovement playerMovement;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Assumes the player object has a "Player" tag
        originalPosition = transform.position;
        playerMovement = player.GetComponent<PlayerMovement>(); // Get the PlayerMovement script
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if(isMoving) 
            MoveTowardsPlayer();
        CheckAttackPlayer();
    }
       private int GetDirectionIndex(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        if (angle >= 337.5f || angle < 22.5f)
            return 0; // Right
        else if (angle >= 22.5f && angle < 67.5f)
            return 1; // Up Right
        else if (angle >= 67.5f && angle < 112.5f)
            return 2; // Up
        else if (angle >= 112.5f && angle < 157.5f)
            return 3; // Up Left
        else if (angle >= 157.5f && angle < 202.5f)
            return 4; // Left
        else if (angle >= 202.5f && angle < 247.5f)
            return 5; // Down Left
        else if (angle >= 247.5f && angle < 292.5f)
            return 6; // Down
        else
            return 7; // Down Right
    }


    private void MoveTowardsPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;

        if (direction.magnitude > 0.01f)
        {
            Vector2 moveDirection = direction.normalized;
            int directionIndex = GetDirectionIndex(moveDirection);
            PlayWalkAnimation(directionIndex);
            lastMoveDirection = moveDirection;
        }
        else
        {
            PlayIdleAnimation();
        }

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }
    private void PlayWalkAnimation(int direction)
    {
        switch (direction)
        {
            case 0: animator.Play(walkRight); break;
            case 1: animator.Play(walkUpRight); break;
            case 2: animator.Play(walkUp); break;
            case 3: animator.Play(walkUpLeft); break;
            case 4: animator.Play(walkLeft); break;
            case 5: animator.Play(walkDownLeft); break;
            case 6: animator.Play(walkDown); break;
            case 7: animator.Play(walkDownRight); break;
        }
    }
    private void PlayIdleAnimation()
    {
        float angle = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        if (angle >= 337.5f || angle < 22.5f)
            animator.Play(idleRight);
        else if (angle >= 22.5f && angle < 157.5f)
            animator.Play(idleUp);
        else if (angle >= 157.5f && angle < 202.5f)
            animator.Play(idleLeft);
        else
            animator.Play(idleDown);
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
        PlayIdleAnimation();
    
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
