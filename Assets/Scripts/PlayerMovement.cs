using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
   // Constants
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

    // UI References
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
    public TMP_Text levelDamage;
    public TMP_Text levelSpeed;
    public TMP_Text levelRange;
    public TMP_Text depositText; 
    public Transform BarrelTransform;

    // Upgrade Levels and Max Levels
    private int damageLevel = 1;
    private int speedLevel = 1;
    private int rangeLevel = 1;
    private int maxDamageLevel = 10;
    private int maxSpeedLevel = 10;
    private int maxRangeLevel = 10;
    public TMP_Text costDamageUpgrade;
    public TMP_Text costSpeedUpgrade;
    public TMP_Text costRangeUpgrade;

    // Player Properties
    public float laserLength = 5f;
    public float moveSpeed;
    public int damageAmount = 1;
    public float attackInterval = 0.5f;
    public float time = 10.0f; 
    public float health = 100; 
    // Player State
    private Vector2 moveInput;
    private int coins = 0;
    private int TotalCoins = 0;
    private int direction = 0;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    public GameObject projectilePrefab;  // Prefab of the projectile

    // Other
    public Rigidbody2D rb2d;
    public LayerMask materialLayer;
    public float raycastDistance = 5f;
    public float raycastLineWidthstart = 0.2f;
    public float raycastLineWidthend = 0.1f;
    public Color rayColor = Color.red;
    public Animator animator;
    public LineRenderer lineRenderer;
    private Vector2 lastMoveDirection;
    private Vector3 lastPosition;
    public Dictionary<string, int> playerMaterials = new Dictionary<string, int>();
    public static PlayerMovement Instance;
    public TMP_Text upgradeMessageText; // New UI text element for upgrade messages
    Vector2 startPos;
public float knockbackForce = 100f; // Adjust the value as needed


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
        DontDestroyOnLoad(gameObject);
        lineRenderer.startWidth = raycastLineWidthstart;
        lineRenderer.enabled = false;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        startPos = transform.position;
        UpdateUpgradeCosts();

    }

   
    void Update()
    {
        UpdateUpgradeCosts();
        UpdatePlayerUI();
        HandlePlayerMovement();
        HandlePlayerAction();
        UpdateMaterialCounts();
    }
   void UpdateUpgradeCosts()
    {
        costDamageUpgrade.text = "Upgrade Cost: " + (100 * damageLevel).ToString();
        costSpeedUpgrade.text = "Upgrade Cost: " + (100 * speedLevel).ToString();
        costRangeUpgrade.text = "Upgrade Cost: " + (100 * rangeLevel).ToString();
    }
    void UpdatePlayerUI()
    {
        coinText.text = TotalCoins.ToString();
        coinEndCount.text = TotalCoins.ToString();
        coinUpgradeCount.text = TotalCoins.ToString();
        lineRenderer.endWidth = raycastLineWidthend;
    }

    void HandlePlayerMovement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        rb2d.velocity = moveInput * moveSpeed;

        if (moveInput != Vector2.zero)
        {
            lastPosition = transform.position;
        }

        handleAnimation();
        float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

        if (angle >= -22.5f && angle < 22.5f)
        {
            direction = 0;
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            direction = 1;
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            direction = 2;
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            direction = 3;
        }
        else if (angle >= 157.5f || angle < -157.5f)
        {
            direction = 4;
        }
        else if (angle >= -157.5f && angle < -112.5f)
        {
            direction = 5;
        }
        else if (angle >= -112.5f && angle < -67.5f)
        {
            direction = 6;
        }
        else if (angle >= -67.5f && angle < -22.5f)
        {
            direction = 7;
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
public void TakeDamage(float amount, GameObject enemy)
{
    Debug.Log("Taking damage: " + amount);
  health -= amount;

        if (health <= 0)
        {
            //TODO: ADD Death functionality
        }
    }

public void Knockback(Vector3 enemyPosition, float knockbackForce)
{
    Vector2 direction = transform.position - enemyPosition;
    direction.Normalize();
    // Try adding the knockback force over a few frames
    StartCoroutine(ApplyKnockback(direction, knockbackForce));
    Debug.Log("Player should get knocked back");
}

private IEnumerator ApplyKnockback(Vector2 direction, float force)
{
    float knockbackTime = 0.2f; // The time over which the knockback occurs
    while (knockbackTime > 0f)
    {
        rb2d.AddForce(direction * (force / Time.deltaTime), ForceMode2D.Force); // Apply the force divided over each frame
        knockbackTime -= Time.deltaTime;
        yield return null;
    }
}

    void HandlePlayerAction()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mousePosition - transform.position).normalized;
                Vector2 aimDirection = directionToMouse;
    
        if (Input.GetMouseButtonDown(0))  // Left mouse button
        {
            FireProjectile(directionToMouse);
        }
               if (Input.GetMouseButton(1))
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
                EnemyController enemyController = hit.collider.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    
                    enemyController.TakeDamage(damageAmount);
                }
            }
        }
    }
}

 void FireProjectile(Vector2 direction)
    {
    GameObject projectile = Instantiate(projectilePrefab, BarrelTransform.position, Quaternion.identity);
    Projectile projectileComponent = projectile.GetComponent<Projectile>();
    projectileComponent.Fire(direction);

    // Calculate the angle in degrees between the direction vector and the positive X axis
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    // Rotate the projectile to face the firing direction
    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    void UpdateMaterialCounts()
    {
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
        if (TotalCoins >= cost)
        {
            // Subtract cost
            TotalCoins -= cost;

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

                 // Show success message
            upgradeMessageText.text = "Upgrade succeeded!";
            upgradeMessageText.color = Color.green;

            return true;
        }
        else
        {
            // Show failure message
            upgradeMessageText.text = "Not enough coins for upgrade";
            upgradeMessageText.color = Color.red;

            return false;
        }
    }
  public void upgradeDamage()
    {
        if (damageLevel >= maxDamageLevel) return;
        int cost = 100 * damageLevel; 
        float upgradeAmount = 2f / damageLevel; 
        if (TryUpgrade("damageAmount", cost, upgradeAmount))
        {
            damageLevel++; 
            coinUpgradeCount.text = TotalCoins.ToString();
            upgradeMessageText.text = "Upgrade succeeded! Damage Amount = " + damageAmount;
            upgradeMessageText.color = Color.green;
            levelDamage.text = "Damage " + damageAmount;
             UpdateUpgradeCosts();
        }
        else
        {
            upgradeMessageText.text = "Upgrade failed! Not enough coins.";
            upgradeMessageText.color = Color.red;
        }
    }

  public void upgradeLaserLength()
    {
        if (rangeLevel >= maxRangeLevel) return;
        int cost = 100 * rangeLevel; 
        float upgradeAmount = 1f / rangeLevel;
        if (TryUpgrade("laserLength", cost, upgradeAmount))
        {
            rangeLevel++;
            coinUpgradeCount.text = TotalCoins.ToString();
            upgradeMessageText.text = "Upgrade succeeded! Laser Length = " + laserLength;
            upgradeMessageText.color = Color.green;
            levelRange.text = "Range " + laserLength;
               UpdateUpgradeCosts();
        }
        else
        {
            upgradeMessageText.text = "Upgrade failed! Not enough coins.";
            upgradeMessageText.color = Color.red;
        }
    }

    public void upgradeMoveSpeed()
    {
        if (speedLevel >= maxSpeedLevel) return;
        int cost = 100 * speedLevel; 
        float upgradeAmount = 1f / speedLevel;
        if (TryUpgrade("moveSpeed", cost, upgradeAmount))
        {
            speedLevel++;
            coinUpgradeCount.text = TotalCoins.ToString();
            upgradeMessageText.text = "Upgrade succeeded! Move Speed = " + moveSpeed;
            upgradeMessageText.color = Color.green;
            levelSpeed.text = "Speed " + moveSpeed;
               UpdateUpgradeCosts();
        }
        else
        {
            upgradeMessageText.text = "Upgrade failed! Not enough coins.";
            upgradeMessageText.color = Color.red;
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
 void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            depositText.gameObject.SetActive(true);
        }
    }
   
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            depositText.gameObject.SetActive(false);
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