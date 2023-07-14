using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Transform shipTransform; // Reference to the ship's transform
    public LineRenderer lineRenderer; // Reference to the LineRenderer component

    private void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        
    }

    private void Update()
    {
        // Calculate the position for the line renderer at the character's belly
        Vector3 bellyPosition = transform.position + new Vector3(0f, -0.5f, 0f);

        // Update the positions of the line renderer
        lineRenderer.SetPosition(0, bellyPosition);
        lineRenderer.SetPosition(1, shipTransform.position);
    }
}
