using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;

    public LayerMask materialLayer;
    public float raycastDistance = 5f;
    public float raycastLineWidth = 0.1f;
    public int damageAmount = 1;

    private LineRenderer lineRenderer;
    private MaterialController currentMaterialController;

    private Vector2 lastMoveDirection;
    private Vector3 lastPosition;

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
            lastMoveDirection = moveInput;
            lastPosition = transform.position;
        }

        RaycastHit2D hit = Physics2D.Raycast(lastPosition, lastMoveDirection, raycastDistance, materialLayer);
        Debug.DrawRay(lastPosition, lastMoveDirection * raycastDistance, Color.green);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Material"))
        {
           // Debug.Log("Hit Material: " + hit.collider.gameObject.name);
            currentMaterialController = hit.collider.gameObject.GetComponent<MaterialController>();

            if (Input.GetMouseButtonDown(0))
            {
                currentMaterialController.TakeDamage(damageAmount);
            }
        }
        else
        {
            currentMaterialController = null;
        }

        UpdateLineRenderer();
    }

    void UpdateLineRenderer()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, lastPosition);
            lineRenderer.SetPosition(1, lastPosition + (Vector3)lastMoveDirection.normalized * raycastDistance);
        }
    }
}
