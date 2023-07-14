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
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // calculate the distance from the player
        float distance = Vector2.Distance(playerTransform.position, transform.position);

        // if the loot is within the attraction radius
        if (distance <= attractionRadius)
        {
            // move the loot towards the player
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, attractionSpeed * Time.deltaTime);
        }

        // if the loot has reached the player
        if (distance <= 0)
        {
            // add the loot's value to the player's coins
            playerTransform.GetComponent<PlayerMovement>().AddCoins(value);

            // destroy the loot
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
