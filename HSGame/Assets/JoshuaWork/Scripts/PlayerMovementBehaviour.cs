using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovementBehaviour : MonoBehaviour
{
    [SerializeField]
    float beginVJumpForce = 15f;
    [SerializeField]
    float floorVJumpForce = 5f;
    private float alteredVJumpForce = 0f;
    [SerializeField]
    float beginHJumpForce = 5f;
    [SerializeField]
    float capHJumpForce = 15f;
    private float alteredHJumpForce = 0f;
    [SerializeField]
    float moveSpeed = 5f; // Adjustable movement speed in the Inspector
    [SerializeField]
    float clamberForce = 10;

    [SerializeField]
    float groundedRayLength = 1f;

    [SerializeField]
    GameObject wallCollision;
    bool touchingWall = false;
    bool isClambering = false;
    bool hasClambered = false;

    [SerializeField]
    Image jumpChargeBar;

    bool isGrounded = false ;
    bool chargingJump = false;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    InputAction moveAction;

    [SerializeField]
    InputAction jumpAction;

    /*Raycast Layermask*/
    [SerializeField]
    LayerMask groundRayMask;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on this GameObject!");
            enabled = false; // Disable the script if no Rigidbody is found
        }

        /*Initailize Altered Jump Directions*/
        alteredVJumpForce = beginVJumpForce;
        alteredHJumpForce = beginHJumpForce;

    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void FixedUpdate()
    {

        /*  Movement  */
        // Get input for horizontal and vertical movement
        Vector2 movementVector = moveAction.ReadValue<Vector2>();

        // Calculate movement direction based on input
        Vector3 movement = new Vector3(movementVector.x, 0f, movementVector.y);

        Vector3 currentVelocity = rb.velocity;

        //movement
        if ((movement.x != 0 || movement.z != 0) && isGrounded && !chargingJump)
        {
            rb.velocity = new Vector3(movement.x * moveSpeed, currentVelocity.y, movement.z * moveSpeed);
            //transform.rotation = Quaternion.LookRotation(movement);
        }
        //stop momentum if grounded
        else if (movement.x == 0 && movement.z == 0 && isGrounded && !chargingJump)
        {
            rb.velocity = new Vector3(movement.x * moveSpeed, 0, movement.z * moveSpeed);
        }

        /*  isGrounded Logic  */

        //Ground Detection RayCast
        RaycastHit hit;

        
        //Make this a box cast to preserve my sanity maybe
        //naw thats lameeeee go my two million raycasts

        //Raycast Debug lines :)
        Debug.DrawLine(transform.position + (transform.forward * 0.5f), transform.position + (transform.forward * 0.5f) + new Vector3(0, -1f * groundedRayLength, 0), Color.blue);
        Debug.DrawLine(transform.position - (transform.forward * 0.5f), transform.position - (transform.forward * 0.5f) + new Vector3(0, -1f * groundedRayLength, 0), Color.blue);

        //if not moving on the y vector, and not charging a jump, and either raycasts is hitting ground...
        //player is grounded
        if (rb.velocity.y < 0.01 && rb.velocity.y > -0.01 && !chargingJump && (Physics.Raycast(transform.position + (transform.forward * 0.5f), Vector3.down, out hit, groundedRayLength, groundRayMask) || Physics.Raycast(transform.position - (transform.forward * 0.5f), Vector3.down, out hit, groundedRayLength, groundRayMask)))
        {
            isGrounded = true;
            isClambering = false;
            hasClambered = false;
        }
        // if moving in the vertical direction and raycast is not touching the floor
        else if ((rb.velocity.y > 0.01 || rb.velocity.y < -0.01) && !(Physics.Raycast(transform.position + (transform.forward * 0.5f), Vector3.down, out hit, groundedRayLength, groundRayMask) || Physics.Raycast(transform.position - (transform.forward * 0.5f), Vector3.down, out hit, groundedRayLength, groundRayMask)))
        {
            isGrounded = false;
        }
        // if raycast is not touching the floor
        else if (!(Physics.Raycast(transform.position + (transform.forward * 0.5f), Vector3.down, out hit, groundedRayLength, groundRayMask) || Physics.Raycast(transform.position - (transform.forward * 0.5f), Vector3.down, out hit, groundedRayLength, groundRayMask)))
        {
            isGrounded = false;
        }
        /*
        //accursed slope logic
        else if ((rb.velocity.y > 0.01 || rb.velocity.y < -0.01) &&!chargingJump && (Physics.Raycast(transform.position + (transform.forward * 0.5f), Vector3.down, out hit, groundedRayLength, groundRayMask) || Physics.Raycast(transform.position - (transform.forward * 0.5f), Vector3.down, out hit, groundedRayLength, groundRayMask))) 
        {
            isGrounded = true;
            isClambering = false;
            hasClambered = false;
        }
        */


        /* Jump Charging */
        if (jumpAction.ReadValue<float>() == 1f && isGrounded)
        {

            rb.velocity = new Vector3(0, currentVelocity.y, 0);

            if (!chargingJump) chargingJump = true;
            alteredHJumpForce += .2f;
            alteredVJumpForce -= .2f;

            if (alteredHJumpForce > capHJumpForce)
            {
                alteredHJumpForce = capHJumpForce;
            }
            if (alteredVJumpForce < floorVJumpForce)
            {
                alteredVJumpForce = floorVJumpForce;
            }

            jumpChargeBar.fillAmount = (alteredHJumpForce - beginHJumpForce) / (capHJumpForce - beginHJumpForce);

        }

        /* Jump Itself */
        if (jumpAction.ReadValue<float>() == 0f && chargingJump)
        {
            isGrounded = false; // Player is no longer grounded after jumping
            // Apply an upward force to the Rigidbody
            rb.AddForce(Vector3.up * alteredVJumpForce, ForceMode.Impulse);
            rb.AddForce(transform.forward * alteredHJumpForce, ForceMode.Impulse);
            chargingJump = false;

            alteredHJumpForce = beginHJumpForce;
            alteredVJumpForce = beginVJumpForce;
            jumpChargeBar.fillAmount = 0f;
        }

        /* Wall Clamber */

        //if wall detection and jump and not grounded, clamber shenanigans :)

        if (jumpAction.ReadValue<float>() == 1f && touchingWall && !isGrounded && !hasClambered)
        {
            
            rb.velocity = new Vector3(0, clamberForce, 0);

            hasClambered = true;
            isClambering = true;

        }

        if (isClambering && !touchingWall && !isGrounded && rb.velocity.y > 0) {
            rb.velocity = Vector3.zero;
            rb.velocity = transform.forward * 10;

            isClambering = false;
        }

        Debug.Log("IsClamber: " + isClambering);
        Debug.Log("HasClamber: " + hasClambered);
        Debug.Log("Grounded: " + isGrounded);
        Debug.Log("TouchWall: " + touchingWall);

    }

    //TriggerEnter & Exit

    private void OnTriggerEnter(Collider other)
    {
        if (touchingWall == false)
        {
            touchingWall = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Touching the Wall :D");
        touchingWall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (touchingWall == true)
        {
            touchingWall = false;
        }
    }
}