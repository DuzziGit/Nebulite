using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;  // The speed of the projectile

    public void Fire(Vector2 direction)
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.velocity = direction.normalized * speed;
        }
    }
void OnTriggerEnter2D(Collider2D other)
{
    // Check if the object collided with is an enemy or other intended target
    if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Material"))
    {
        MaterialController materialController = other.GetComponent<MaterialController>();
        if (materialController != null)
        {
            if (PlayerMovement.Instance != null)
            {
                materialController.TakeDamage(PlayerMovement.Instance.damageAmount);
            }
        }
    
        EnemyController enemyController = other.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            if (PlayerMovement.Instance != null)
            {
                enemyController.TakeDamage(PlayerMovement.Instance.damageAmount);
            }
        }

        Destroy(gameObject);
    }
}

}
