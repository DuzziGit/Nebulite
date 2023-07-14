using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public enum LaserDirection
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    [System.Serializable]
    public struct DirectionalSprite
    {
        public LaserDirection direction;
        public Sprite sprite;
    }

    public float radius;
    public float speed;
    public GameObject player;
    public SpriteRenderer spriteRenderer;
    public DirectionalSprite[] directionSprites;

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
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.up = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360;
        }

        LaserDirection directionIndex = (LaserDirection)(Mathf.FloorToInt(angle / 45) % 8);
        Sprite spriteToUse = null;

        for (int i = 0; i < directionSprites.Length; i++)
        {
            if (directionSprites[i].direction == directionIndex)
            {
                spriteToUse = directionSprites[i].sprite;
                break;
            }
        }

        if (spriteToUse != null)
        {
            spriteRenderer.sprite = spriteToUse;
        }

        // Keep laser at a constant distance from player
        transform.position = player.transform.position + (transform.up * radius);
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
