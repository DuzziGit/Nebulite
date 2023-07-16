using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public LineRenderer lineRenderer; // Reference to the LineRenderer component
    private Transform shipTransform; // Reference to the ship's transform

    private void Start()
    {
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // Find the GameObject with the name "Ship" and assign its transform to shipTransform
        GameObject shipObject = GameObject.Find("Ship");
        if (shipObject != null)
        {
            shipTransform = shipObject.transform;
        }
        else
        {
            Debug.LogError("Ship GameObject not found in the scene!");
        }
    }

    private void Update()
    {
        // Ensure shipTransform is not null before updating the line renderer
        if (shipTransform != null)
        {
            // Calculate the position for the line renderer at the character's belly
            Vector3 bellyPosition = transform.position + new Vector3(0f, -0.5f, 0f);

            // Update the positions of the line renderer
            lineRenderer.SetPosition(0, bellyPosition);
            lineRenderer.SetPosition(1, shipTransform.position);
        }
    }
}
