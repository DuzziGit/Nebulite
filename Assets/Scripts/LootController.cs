using UnityEngine;

public class LootController : MonoBehaviour
{
    public int value; // the value of the loot
    public string materialType; // the type of the loot

    public float attractionRadius = 5f; // the radius within which the loot will start moving towards the player
    public float attractionSpeed = 2f; // the speed at which the loot will move towards the player

    private Transform playerTransform;

    void Start()
    {
        // Find the player object and get its transform
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;

        // Get the PlayerMovement component attached to the player object
        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();

        // Increase the attraction speed by 1 using the player's moveSpeed property
        if (playerMovement != null)
        {
           attractionSpeed = playerMovement.moveSpeed + 1;
        }
    }

    void Update()
    {
        // Calculate the distance from the player
        float distance = Vector2.Distance(playerTransform.position, transform.position);

        // If the loot is within the attraction radius
        if (distance <= attractionRadius)
        {
            // Move the loot towards the player
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, attractionSpeed * Time.deltaTime);
        }

        // If the loot has reached the player
        if (distance <= 0)
        {
            // Get the PlayerMovement component
            PlayerMovement playerMovement = playerTransform.GetComponent<PlayerMovement>();

            // Add the loot's value to the player's coins
            playerMovement.AddCoins(value);

            // Add the material to the player's materials
            playerMovement.AddMaterial(materialType);

            // Destroy the loot
            Destroy(gameObject);
        }
    }


    void OnDrawGizmosSelected()
    {
        // Draw a wire sphere representing the attraction radius in the scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
