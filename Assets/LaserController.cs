using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{




    public float radius;
    public float speed;
    public GameObject player;
    public SpriteRenderer spriteRenderer;

    private Vector3 mousePosition;
    private float angle;
    private int direction;

    void Update()
    {
        RotateLaser();
    }

    void RotateLaser()
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(
            mousePosition.x - player.transform.position.x,
            mousePosition.y - player.transform.position.y
        );

        // Check if the direction vector's magnitude is greater than the deadzone value
        float distanceToPlayer = Vector2.Distance(player.transform.position, mousePosition);
        float deadzone = 0.1f; // Adjust this to suit your needs

        if (distanceToPlayer > deadzone)
        {
            // Normalize the direction vector so its length is 1
            direction = direction.normalized;

            // Calculate the new position of the laser, which is in the direction of the mouse but capped at the radius
            Vector3 laserPosition = player.transform.position + (Vector3)direction * Mathf.Min(distanceToPlayer, radius);

            // Rotate and position the laser
            transform.up = direction;
            transform.position = laserPosition;

            // Calculate the angle and sprite based on the direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

          
        }
    }


    void OnDrawGizmos()
    {
        // Draws a blue line from this transform to the player
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, player.transform.position);

        // Draws a blue circle at this transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, radius);
    }
}
