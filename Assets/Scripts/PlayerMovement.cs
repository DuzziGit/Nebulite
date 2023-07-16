using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    const string idleUp = "IdleAnimUp";
    const string idleDown = "IdleAnimDown";
    const string idleRight = "IdleAnimRight";
    const string idleLeft = "IdleAnimLeft";

    const string walkUp = "WalkUp";
    const string walkDown = "WalkDown";
    const string walkRight = "WalkSideRight";
    const string walkLeft = "WalkSideLeft";
    const string walkUpRight = "WalkUpRight";
    const string walkUpLeft = "WalkUpLeft";
    const string walkDownRight = "WalkDownRight";
    const string walkDownLeft = "WalkDownLeft";
    public GameObject laserStartPoint;

    public GameObject timerController;
	public UIController uiController;

	public TMP_Text mat1GuiCount;
	public TMP_Text mat2GuiCount;
	public TMP_Text mat3GuiCount;
	public TMP_Text mat4GuiCount;
	public TMP_Text mat5GuiCount;

    
    public TMP_Text mat1EndCount;
	public TMP_Text mat2EndCount;
	public TMP_Text mat3EndCount;
	public TMP_Text mat4EndCount;
	public TMP_Text mat5EndCount;

    public TMP_Text coinText;
    public TMP_Text coinEndCount;
    public TMP_Text coinUpgradeCount;







	public float laserLength = 5f;

    public float moveSpeed;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;
    public LayerMask materialLayer;
    public float raycastDistance = 5f;
    public float raycastLineWidthstart = 0.2f;
    public float raycastLineWidthend = 0.1f;
    public Color rayColor = Color.red;

    public int damageAmount = 1;
    public float attackInterval = 0.5f;
    public float time = 10.0f; //We Should change the name of this later


    // public Color rayColor = Color.red;
    public Animator animator;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    public LineRenderer lineRenderer;
    private Vector2 lastMoveDirection;
    private Vector3 lastPosition;
    private int coins = 0;
    private int TotalCoins = 0;
    private int direction = 0;
    public Dictionary<string, int> playerMaterials = new Dictionary<string, int>();
    public static PlayerMovement Instance;

    Vector2 startPos;


  void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // Find or create a LineRenderer component
        DontDestroyOnLoad(gameObject);


        lineRenderer.startWidth = raycastLineWidthstart;
       // lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
//lineRenderer.material = Resources.Load<Material>("LaserMat");
        lineRenderer.enabled = false;

		rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		startPos = transform.position;
    }

    void Update()
    {

        coinText.text = TotalCoins.ToString();
        coinEndCount.text = TotalCoins.ToString();
        coinUpgradeCount.text = TotalCoins.ToString();
        lineRenderer.endWidth = raycastLineWidthend;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed;

        // Update animator parameters

        if (moveInput != Vector2.zero)
        {
            lastPosition = transform.position;

        }

        handleAnimation();
        // Calculate the angle based on moveInput
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
        else
        {
            animator.SetBool("IsWalking", false);
        }

        // Aim towards the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mousePosition - transform.position).normalized;
        Vector2 aimDirection = directionToMouse;

        // Attack control
        if (Input.GetMouseButton(0))
        {
            isAttacking = true;
            lineRenderer.enabled = true;

            Vector3 laserStart = laserStartPoint != null ? laserStartPoint.transform.position : transform.position;
            Vector3 laserEnd = laserStart + (new Vector3(aimDirection.x, aimDirection.y, 0) * laserLength);

            lineRenderer.SetPosition(0, laserStart);
            lineRenderer.SetPosition(1, laserEnd);
        }
        else
        {
            isAttacking = false;
            lineRenderer.enabled = false;
        }


        // Attacking and damaging the material
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, raycastDistance, materialLayer);
                if (hit.collider != null)
                {
                    MaterialController materialController = hit.collider.GetComponent<MaterialController>();
                    if (materialController != null)
                    {
                        materialController.TakeDamage(damageAmount);
                    }
                }
            }
        }

	if (playerMaterials.ContainsKey("Asteroid"))
{
    mat1GuiCount.text = playerMaterials["Asteroid"].ToString();
    mat1EndCount.text = playerMaterials["Asteroid"].ToString();
}

if (playerMaterials.ContainsKey("Mushroom"))
{
    mat2GuiCount.text = playerMaterials["Mushroom"].ToString();
    mat2EndCount.text = playerMaterials["Mushroom"].ToString();
}

if (playerMaterials.ContainsKey("Geode"))
{
    mat3GuiCount.text = playerMaterials["Geode"].ToString();
    mat3EndCount.text = playerMaterials["Geode"].ToString();
}

if (playerMaterials.ContainsKey("Relic"))
{
    mat4GuiCount.text = playerMaterials["Relic"].ToString();
    mat4EndCount.text = playerMaterials["Relic"].ToString();
}

if (playerMaterials.ContainsKey("Petal"))
{
    mat5GuiCount.text = playerMaterials["Petal"].ToString();
    mat5EndCount.text = playerMaterials["Petal"].ToString();
}
	}

    void handleAnimation()
    {
        // Only play walk animation if character is moving
        if (moveInput.magnitude > 0)
        {
            switch (direction)
            {
                case 0:
                    animator.Play(walkRight);
                    lastMoveDirection = Vector2.right;
                    break;
                case 1:
                    animator.Play(walkUpRight);
                    lastMoveDirection = new Vector2(1f, 1f);
                    break;
                case 2:
                    animator.Play(walkUp);
                    lastMoveDirection = Vector2.up;
                    break;
                case 3:
                    animator.Play(walkUpLeft);
                    lastMoveDirection = new Vector2(-1f, 1f);
                    break;
                case 4:
                    animator.Play(walkLeft);
                    lastMoveDirection = Vector2.left;
                    break;
                case 5:
                    animator.Play(walkDownLeft);
                    lastMoveDirection = new Vector2(-1f, -1f);
                    break;
                case 6:
                    animator.Play(walkDown);
                    lastMoveDirection = Vector2.down;
                    break;
                case 7:
                    animator.Play(walkDownRight);
                    lastMoveDirection = new Vector2(1f, -1f);
                    break;
            }
        }
        else
        {
            // Play idle animation based on last move direction
            float angle = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;

            if (angle >= -22.5f && angle < 22.5f)
            {
                // Right
                animator.Play(idleRight);
            }
            else if (angle >= 22.5f && angle < 112.5f)
            {
                // Up Right to Up Left
                animator.Play(idleUp);
            }
            else if (angle >= 112.5f && angle < 157.5f || angle >= -180f && angle < -157.5f)
            {
                // Left
                animator.Play(idleLeft);
            }
            else
            {
                // Down
                animator.Play(idleDown);
            }
        }
    }

        public void AddCoins(int amount)
    {
        coins += amount;
    }

    public void AddMaterial(string materialType)
    {
        if (playerMaterials.ContainsKey(materialType))
        {
            // If the material is already in the dictionary, increment the count
            playerMaterials[materialType]++;
        }
        else
        {
            // Otherwise, add the material to the dictionary with a count of 1
            playerMaterials.Add(materialType, 1);
        }
       // Debug.Log("Material: " + materialType + ", Count: " + playerMaterials[materialType]);

    }
    public bool TryUpgrade(string property, int cost, float upgradeAmount)
    {
        // Check if player has enough coins
        if (coins >= cost)
        {
            // Subtract cost
            coins -= cost;

            // Upgrade the desired property
            switch (property)
            {
                case "laserLength":
                    laserLength += upgradeAmount;
                    raycastDistance += upgradeAmount; // Update the raycast distance along with the visual laser length
                    raycastLineWidthend += .2f;
                    break;
                case "damageAmount":
                    damageAmount += (int)upgradeAmount;  // assuming that damageAmount should remain an integer
                    break;
                case "moveSpeed":
                    moveSpeed += upgradeAmount;
                    break;
                default:
            //        Debug.LogWarning("Unknown property: " + property);
                    return false;
            }

            return true;
        }
        else
        {
           // Debug.Log("Not enough coins for upgrade");
            Debug.Log("Upgrade failed");

            return false;
        }
    }
    public void upgradeDamage()
    {
        if (TryUpgrade("damageAmount", 100, 2))
        {
            Debug.Log("Upgrade succeeded");
            Debug.Log("Damage Amount = " + damageAmount);
        }
    }

    public void upgradeLaserLength()
    {
        if (TryUpgrade("laserLength", 100, 1))
        {
            Debug.Log("Upgrade succeeded");
            Debug.Log("Laser Length = " + laserLength);
        }
    }

    public void upgradeMoveSpeed()
    {
        if (TryUpgrade("moveSpeed", 100, 1))
        {
            Debug.Log("Upgrade succeeded");
            Debug.Log("moveSpeed = " + moveSpeed);
        }
    }

	public void TimeHasRunOut()
	{
		Freeze();

		uiController.PlayerLoss();
		
	}



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Door") && Input.GetKey(KeyCode.E))
        {

			TotalCoins += coins;
          //  Debug.Log("TotalCoins: " + TotalCoins);
            coins = 0;
          //  Debug.Log("Temp coins erased: " + coins);

            coinText.text = TotalCoins.ToString();
            coinEndCount.text = TotalCoins.ToString();
            
            WipeMaterialsInGame();
            Freeze();
			uiController.PlayerWin();

           
        }
    }

  public void DisplayDeathInfoUI()
    {

      
		// Find the DeathInfoUI object
		GameObject deathInfoUI = transform.Find("DeathInfoUI").gameObject;

		// Disable the DeathInfoUI object
		deathInfoUI.SetActive(true);


	}

	public void HideDeathInfoUI()
	{


		// Find the DeathInfoUI object
		GameObject deathInfoUI = transform.Find("DeathInfoUI").gameObject;

		// Enable the DeathInfoUI object
		deathInfoUI.SetActive(false);
	}

	public void Freeze()
	{
		rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

	}

	public void Unfreeze()
    {
        rb2d.constraints &= RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionY;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

    public void Reposition()
    {
		transform.position = startPos;
	}


    public void WipeMaterials()
    {
		playerMaterials.Clear();
        coins = 0;

		mat1GuiCount.text = "0";
		mat1EndCount.text = "0";

		mat2GuiCount.text = "0";
		mat2EndCount.text = "0";

	    mat3GuiCount.text = "0";
		mat3EndCount.text = "0";

		mat4GuiCount.text = "0";
		mat4EndCount.text = "0";

        mat5GuiCount.text = "0";
		mat5EndCount.text = "0";

	}

	public void WipeMaterialsInGame()
	{
		playerMaterials.Clear();

		
		mat1GuiCount.text = "0";

		mat2GuiCount.text = "0";

	    mat3GuiCount.text = "0";

		mat4GuiCount.text = "0";
        
        mat5GuiCount.text = "0";


	}
}