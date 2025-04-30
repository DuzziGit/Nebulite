using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float radius;                      // Max distance the laser can reach
    public float speed;                       // Not currently used
    public GameObject player;                 // Reference to the player object
    public SpriteRenderer spriteRenderer;     // Optional sprite renderer for visuals

    public float yOffset = 0.5f;              // Vertical offset from player's origin (e.g., chest or head)
    public float zOffsetInFront = -0.1f;      // Z offset to make the laser appear in front of the player
    public float zOffsetBehind = 0.1f;        // Z offset to make the laser appear behind the player

    private Vector3 mousePosition;

    void Update()
    {
        RotateLaser();
        HandleLaserPosition();
    }

    void RotateLaser()
    {
        // Get mouse position in world space
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0f;

        // Offset origin point upward from the player's center
        Vector3 playerAnchor = player.transform.position + new Vector3(0f, yOffset, 0f);

        // Calculate direction from anchor to mouse
        Vector2 direction = new Vector2(
            mousePosition.x - playerAnchor.x,
            mousePosition.y - playerAnchor.y
        );

        float distanceToMouse = direction.magnitude;
        float deadzone = 0.1f; // Prevents flickering when mouse is too close

        if (distanceToMouse > deadzone)
        {
            // Normalize direction vector
            direction = direction.normalized;

            // Calculate laser position (anchored from elevated point)
            Vector3 laserPosition = playerAnchor + (Vector3)direction * Mathf.Min(distanceToMouse, radius);

            // Rotate laser to face the direction
            transform.up = direction;

            // Apply final laser position
            transform.position = laserPosition;
        }
    }

    // Handle laser position based on "W" key input
    void HandleLaserPosition()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W))  // "W" key pressed, move laser in front of player
        {
            pos.z = player.transform.position.z + zOffsetBehind;  // Move laser in front
        }
        else  // "W" key not pressed, move laser behind the player
        {
            pos.z = player.transform.position.z + zOffsetInFront;  // Move laser behind
        }

        // Update laser position
        transform.position = pos;
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector3 playerAnchor = player.transform.position + new Vector3(0f, yOffset, 0f);

            // Line from laser to player anchor point
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, playerAnchor);

            // Circle around the anchor point for visual radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerAnchor, radius);
        }
    }
}
