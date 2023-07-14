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

    public Animator animator;

    private LineRenderer lineRenderer;
    private MaterialController currentMaterialController;

    private Vector2 lastMoveDirection;
    private Vector3 lastPosition;

    private int coins = 0; // player's coin count
    private Dictionary<string, int> materials = new Dictionary<string, int>(); // player's collected materials

    void Start()
    {
        //lineRenderer = gameObject.AddComponent<LineRenderer>();
        //lineRenderer.startWidth = raycastLineWidth;
        //lineRenderer.endWidth = raycastLineWidth;
        //lineRenderer.material.color = Color.red;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed;

        // Update animator parameters
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetBool("IsWalking", moveInput.magnitude > 0);
        if (moveInput.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
            lastMoveDirection = moveInput;
            lastPosition = transform.position;
            Debug.Log(moveInput.x);
            Debug.Log(moveInput.y);

        } else if (moveInput.magnitude <= 0)
        {
            animator.SetBool("isWalking", false);

        }


        RaycastHit2D hit = Physics2D.Raycast(lastPosition, lastMoveDirection, raycastDistance, materialLayer);
        Debug.DrawRay(lastPosition, lastMoveDirection * raycastDistance, Color.green);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Material"))
        {
            // Debug.Log("Hit Material: " + hit.collider.gameObject.name);
            currentMaterialController = hit.collider.gameObject.GetComponent<MaterialController>();

            if (currentMaterialController != null && Input.GetMouseButtonDown(0))
            {
                currentMaterialController.TakeDamage(damageAmount);
            }
        }
        else
        {
            currentMaterialController = null;
        }

        UpdateLineRenderer();

        Debug.Log("Coins: " + coins);
        foreach (KeyValuePair<string, int> material in materials)
        {
            Debug.Log("Material: " + material.Key + ", Amount: " + material.Value);
        }
    }
    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public void AddMaterial(string type, int amount)
    {
        if (!materials.ContainsKey(type))
        {
            materials[type] = 0;
        }
        materials[type] += amount;
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
