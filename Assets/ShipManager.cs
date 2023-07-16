using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager instance; // Singleton instance

    private Transform shipTransform; // Reference to the ship Transform

    private void Awake()
    {
        // Ensure only one instance of ShipManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Set the shipTransform here
            SetShipTransform(transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetShipTransform(Transform ship)
    {
        shipTransform = ship;
    }

    public Transform GetShipTransform()
    {
        return shipTransform;
    }
}
