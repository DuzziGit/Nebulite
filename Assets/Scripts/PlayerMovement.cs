using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;

    public LayerMask materialLayer;
    public float raycastDistance = 1f;
    public float raycastLineWidth = 0.1f;

    private LineRenderer lineRenderer;
    private GameObject currentMaterial;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = raycastLineWidth;
        lineRenderer.endWidth = raycastLineWidth;
        lineRenderer.material.color = Color.red;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed;

        if (moveInput.magnitude > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveInput, raycastDistance, materialLayer);
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Material"))
            {
                Debug.Log("Hit Material");
                currentMaterial = hit.collider.gameObject;
            }
            else
            {
                currentMaterial = null;
            }

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + (Vector3)moveInput.normalized * raycastDistance);

            if (Input.GetKeyDown(KeyCode.E) && currentMaterial != null)
            {
                Destroy(currentMaterial);
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
