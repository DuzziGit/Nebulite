using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public LineRenderer lineRenderer; // Reference to the LineRenderer component
    private Transform shipTransform; // Reference to the ship's transform

    private void Start()
    {
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get reference to the shipTransform from ShipManager
        shipTransform = ShipManager.instance.GetShipTransform();

        if (shipTransform == null)
        {
            Debug.LogError("Ship Transform not found in the scene!");
        }
    }

    private void Update()
    {
        // Ensure shipTransform is not null before updating the line renderer
        if (shipTransform != null)
        {
            // Calculate the position for the line renderer at the character's belly
            Vector3 bellyPosition = transform.position + new Vector3(0f, -.3f, 0f);

            // Update the positions of the line renderer
            lineRenderer.SetPosition(0, bellyPosition);
            lineRenderer.SetPosition(1, shipTransform.position);
        }
    }
}
