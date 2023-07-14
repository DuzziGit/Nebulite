using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;

    //public BoxCollider2D range;

    public GameObject activeRangeUp;
    public GameObject activeRangeDown;
    public GameObject activeRangeLeft;
    public GameObject activeRangeRight;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        //Get Player move direction
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed;
        //Multiply move direction by moveSpeed value



        if (moveInput.y > 0)
        {
            //Moving up
            Debug.Log("moving up...");
            
            activeRangeUp.SetActive(true);
            activeRangeDown.SetActive(false);
            activeRangeLeft.SetActive(false);
            activeRangeRight.SetActive(false);

        }
        if (moveInput.y < 0)
        {
            //Moving down
            Debug.Log("moving down...");

            activeRangeUp.SetActive(false);
            activeRangeDown.SetActive(true);
            activeRangeLeft.SetActive(false);
            activeRangeRight.SetActive(false);

        }

        if (moveInput.x > 0)
		{
            //Moving right
            Debug.Log("moving right...");

            activeRangeUp.SetActive(false);
            activeRangeDown.SetActive(false);
            activeRangeLeft.SetActive(false);
            activeRangeRight.SetActive(true);

        }
        if (moveInput.x < 0)
        {
            //Moving left
            Debug.Log("moving left...");

            activeRangeUp.SetActive(false);
            activeRangeDown.SetActive(false);
            activeRangeLeft.SetActive(true);
            activeRangeRight.SetActive(false);

        }




    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        if (collision.CompareTag("Material"))
        {

        }

    }
}
