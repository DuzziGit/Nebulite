using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const string idleUp = "idleAnimUp";
    const string idleDown = "idleAnimDown";
    const string idleRight = "idleAnimRight";
    const string idleLeft = "idleAnimLeft";

    const string walkUp = "WalkUp";
    const string walkDown = "WalkDown";
    const string walkRight = "WalkSideRight";
    const string walkLeft = "WalkSideLeft";

    public float moveSpeed;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;
    private int direction;
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
        handleAnimation();
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

            // Calculate the angle of the movement direction
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

            // Set the corresponding animator parameter based on the angle
            if (angle >= -22.5f && angle < 22.5f)
            {
                // Right
                direction = 0;
            }
            else if (angle >= 22.5f && angle < 67.5f)
            {
                // Up Right
                direction = 1;
            }
            else if (angle >= 67.5f && angle < 112.5f)
            {
                // Up
                direction = 2;
            }
            else if (angle >= 112.5f && angle < 157.5f)
            {
                // Up Left
                direction = 3;
            }
            else if (angle >= 157.5f || angle < -157.5f)
            {
                // Left
                direction = 4;
            }
            else if (angle >= -157.5f && angle < -112.5f)
            {
                // Down Left
                direction = 5;
            }
            else if (angle >= -112.5f && angle < -67.5f)
            {
                // Down
                direction = 6;
            }
            else if (angle >= -67.5f && angle < -22.5f)
            {
                // Down Right
                direction = 7;
            }
        }
        else
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

    void handleAnimation()
    {
        if (direction == 0)
        {
            animator.Play(moveInput.magnitude > 0 ? walkRight : idleRight);
        }
        else if (direction == 1)
        {
            animator.Play(moveInput.magnitude > 0 ? walkUp : idleUp);
        }
        else if (direction == 2)
        {
            animator.Play(moveInput.magnitude > 0 ? walkUp : idleUp);
        }
        else if (direction == 3)
        {
            animator.Play(moveInput.magnitude > 0 ? walkUp : idleUp);
        }
        else if (direction == 4)
        {
            animator.Play(moveInput.magnitude > 0 ? walkLeft : idleLeft);
        }
        else if (direction == 5)
        {
            animator.Play(moveInput.magnitude > 0 ? walkDown : idleDown);
        }
        else if (direction == 6)
        {
            animator.Play(moveInput.magnitude > 0 ? walkDown : idleDown);
        }
        else if (direction == 7)
        {
            animator.Play(moveInput.magnitude > 0 ? walkDown : idleDown);
        }
        else
        {
            animator.Play(idleUp);
        }
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
