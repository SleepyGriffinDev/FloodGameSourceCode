using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*

WHAT SHOULD STAY THE SAME:
Grounded Detection, Jumping Logic & Method

WHAT NEEDS TO CHANGE:
How we get the rat to move forward
No more full rotation, prep movement for sprite usage


*/

public class JBNewMovementBehaviour : MonoBehaviour
{

    /*Movement Parameters*/
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

    bool jumpEnabled = true;
    bool groundCheckEnabled = true;
    bool isGrounded = false;
    bool chargingJump = false;
    float jumpCDTimer = 0f;
    readonly float jumpCDTimerFull = 0.5f;
    float groundCheckTimer = 0f;
    readonly float groundCheckTimerFull = 0.5f;

    /*Variables that hold movement-related vectors*/
    Vector3 currentVelocity;
    Vector3 currentMovementDir = Vector3.zero;

    //Length of the ground detection rays
    [SerializeField]
    float groundedRayLength = 1f;
    readonly float rayCastDistance = 0.45f;
    readonly float diagRayCastDistance = 0.31820f;

    /* Variables for Slope Math */
    float dirOfX= 0f;
    Vector3 raycastVec = Vector3.zero;
    Vector3 slopeCrossMult = Vector3.zero;
    Vector3 slopeMovement = Vector3.zero;

    //Clambering related variables
    bool touchingWall = false;
    bool isClambering = false;
    //bool hasClambered = false;

    /* Game Object References */
    [SerializeField]
    Rigidbody rb; //Rat rigidbody

    [SerializeField]
    Image jumpChargeBar; //Bar that fills when charaging jump

    [SerializeField]
    GameObject wallCollisionObject; //Trigger hitbox that does wall detection


    [SerializeField]
    InputAction moveAction;
    [SerializeField]
    InputAction jumpAction;
    [SerializeField]
    InputAction lockMoveAction;
    [SerializeField]
    InputAction cancelJumpAction;

    /* Raycast Layermask */
    [SerializeField]
    LayerMask groundRayMask;

    //Raycast hit data
    RaycastHit groundRayHit;

    //Animator Nonsense
    [SerializeField]
    Animator ratMovementAnimator;

    /*
     */ //Audio Nonsense
    [SerializeField]
    AudioManager audioManager;
    //private bool playingFootsteps = false;
    /*
    [SerializeField]
    private float footstepSpeed = 0.5f;
    */

    /*For controlling the "MoveDir" Animator Parameter*/
    private enum ratFacingDirection
    {
        BACKWARDS = 0,
        BACKWARDS_RIGHT = 1,
        RIGHT = 2,
        FORWARDS_RIGHT = 3,
        FORWARDS = 4,
        FORWARDS_LEFT = 5,
        LEFT = 6,
        BACKWARDS_LEFT = 7
    }
    private ratFacingDirection moveDir = ratFacingDirection.BACKWARDS;

    // Start is called before the first frame update
    void Start()
    {
        /*Initailize Altered Jump Directions*/
        alteredVJumpForce = beginVJumpForce;
        alteredHJumpForce = beginHJumpForce;
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        lockMoveAction.Enable();
        cancelJumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        lockMoveAction.Disable();
        cancelJumpAction.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*  
         *  Movement Logic  
         */

        // Get input for horizontal and vertical movement
        Vector2 movementVector = moveAction.ReadValue<Vector2>();

        // Calculate movement direction based on input
        Vector3 movement = new Vector3(movementVector.x, 0f, movementVector.y);

        if (movement != Vector3.zero && movement != currentMovementDir && isGrounded)
        {
            currentVelocity = rb.velocity;
            rb.velocity = new Vector3(0, currentVelocity.y, 0);
        }

        currentMovementDir = movement;
        if (currentMovementDir.z > 0.9f) //Backwards
        {
            moveDir = ratFacingDirection.BACKWARDS;
        }
        else if ((currentMovementDir.x > 0.7 && currentMovementDir.x < 0.72) &&
                (currentMovementDir.z > 0.7 && currentMovementDir.z < 0.72))
        {
            moveDir = ratFacingDirection.BACKWARDS_RIGHT;
        }
        else if (currentMovementDir.x > 0.9f)
        {
            moveDir = ratFacingDirection.RIGHT;
        }
        else if ((currentMovementDir.x > 0.7 && currentMovementDir.x < 0.72) &&
                (currentMovementDir.z < -0.7 && currentMovementDir.z > -0.72))
        {
            moveDir = ratFacingDirection.FORWARDS_RIGHT;
        }
        else if (currentMovementDir.z < -0.9f)
        {
            moveDir = ratFacingDirection.FORWARDS;
        }
        else if ((currentMovementDir.x < -0.7 && currentMovementDir.x > -0.72) &&
                (currentMovementDir.z < -0.7 && currentMovementDir.z > -0.72))
        {
            moveDir = ratFacingDirection.FORWARDS_LEFT;
        }
        else if (currentMovementDir.x < -0.9)
        {
            moveDir = ratFacingDirection.LEFT;
        }
        else if ((currentMovementDir.x < -0.7 && currentMovementDir.x > -0.72) &&
                (currentMovementDir.z > 0.7 && currentMovementDir.z < 0.72))
        {
            moveDir = ratFacingDirection.BACKWARDS_LEFT;
        }

        ratMovementAnimator.SetInteger("MoveDir", (int)moveDir);
        ratMovementAnimator.SetFloat("MovementX", currentMovementDir.x);
        ratMovementAnimator.SetFloat("MovementZ", currentMovementDir.z);

        //Update Grounded Raycasts for ground's normal
        GetGroundedRaycast(ref groundRayHit);

        if ((movement.x != 0 || movement.z != 0) && lockMoveAction.ReadValue<float>() == 1f && isGrounded && !chargingJump)
        {
            wallCollisionObject.transform.rotation = Quaternion.LookRotation(movement);
        }
        //If moving DOWN a slope, do nightmare vector math 
        else if (groundRayHit.normal.x > 0 && movement.x > 0 && isGrounded && !chargingJump)
        {
            //Math to get the direction of X on normal map
            dirOfX = (groundRayHit.normal.x) / Mathf.Abs(groundRayHit.normal.x);

            //Raycast to simulate the direction of the grounded Raycsts (it just points downward)
            raycastVec = wallCollisionObject.transform.position - new Vector3(wallCollisionObject.transform.position.x, wallCollisionObject.transform.position.y - 1, wallCollisionObject.transform.position.z);

            //Using Cross Multiplication to get a direction parrallel to the slope's angle
            slopeCrossMult = dirOfX * (Vector3.Cross(dirOfX * Vector3.Cross(raycastVec, groundRayHit.normal), groundRayHit.normal));
            slopeCrossMult = slopeCrossMult / slopeCrossMult.magnitude;

            //Adding the Movement Direction to Cross Multiplication product
            slopeMovement = (movement.x / Mathf.Abs(movement.x)) * slopeCrossMult;

            rb.AddForce(new Vector3(slopeMovement.x * moveSpeed, slopeMovement.y * moveSpeed, movement.z), ForceMode.VelocityChange);
            wallCollisionObject.transform.rotation = Quaternion.LookRotation(movement);
        }
        else if (groundRayHit.normal.x < 0 && movement.x < 0 && isGrounded && !chargingJump)
        {
            //Math to get the direction of X on normal map
            dirOfX = (groundRayHit.normal.x) / Mathf.Abs(groundRayHit.normal.x);

            //Raycast to simulate the direction of the grounded Raycsts (it just points downward)
            raycastVec = wallCollisionObject.transform.position - new Vector3(wallCollisionObject.transform.position.x, wallCollisionObject.transform.position.y - 1, wallCollisionObject.transform.position.z);

            //Using Cross Multiplication to get a direction parrallel to the slope's angle
            slopeCrossMult = dirOfX * (Vector3.Cross(dirOfX * Vector3.Cross(raycastVec, groundRayHit.normal), groundRayHit.normal));
            slopeCrossMult = slopeCrossMult / slopeCrossMult.magnitude;

            //Adding the Movement Direction to Cross Multiplication product
            slopeMovement = (movement.x / Mathf.Abs(movement.x)) * slopeCrossMult * -1;

            rb.AddForce(new Vector3(slopeMovement.x * moveSpeed, slopeMovement.y * moveSpeed, movement.z), ForceMode.VelocityChange);
            wallCollisionObject.transform.rotation = Quaternion.LookRotation(movement);
        }
        else if ((movement.x != 0 || movement.z != 0) && isGrounded && !chargingJump)
        {

            rb.AddForce(new Vector3(movement.x * moveSpeed, 0, movement.z * moveSpeed), ForceMode.VelocityChange);

            wallCollisionObject.transform.rotation = Quaternion.LookRotation(movement);
        }
        else if ((movement.x == 0 || movement.z == 0) && isGrounded && !chargingJump)
        {
            currentVelocity = rb.velocity;
            rb.AddForce(new Vector3(-0.5f * currentVelocity.x, -0.5f * currentVelocity.y, -0.5f * currentVelocity.z), ForceMode.VelocityChange);
        }

        /*
         * From Ana's Script. Play footsteps if moving
        if (rb.velocity.magnitude > 0 && !playingFootsteps)
        {
            StartFootsteps();
        }
        else if (rb.velocity.magnitude == 0)
        {
            StopFootsteps();
        }
        */

        //Velocity Caps

        if (isGrounded)
        {
            if (rb.velocity.x > moveSpeed)
            {
                currentVelocity = rb.velocity;
                rb.velocity = new Vector3(moveSpeed, currentVelocity.y, currentVelocity.z);

            }
            else if (rb.velocity.x < (-1 * moveSpeed))
            {
                currentVelocity = rb.velocity;
                rb.velocity = new Vector3(-1 * moveSpeed, currentVelocity.y, currentVelocity.z);

            }
            if (rb.velocity.z > moveSpeed)
            {
                currentVelocity = rb.velocity;
                rb.velocity = new Vector3(currentVelocity.x, currentVelocity.y, moveSpeed);

            }
            else if (rb.velocity.z < (-1 * moveSpeed))
            {
                currentVelocity = rb.velocity;
                rb.velocity = new Vector3(currentVelocity.x, currentVelocity.y, -1 * moveSpeed);

            }
        }

        /*  
         *  isGrounded Logic  
         */

        // if raycast is not touching the floor
        if (!GetGroundedRaycast(ref groundRayHit))
        {
            isGrounded = false;
            ratMovementAnimator.SetBool("isGrounded", isGrounded);
        }
        //if not moving on the y vector, and not charging a jump, and either raycasts is hitting ground...
        else if (rb.velocity.y < 0.01 && rb.velocity.y > -0.01 && !chargingJump && GetGroundedRaycast(ref groundRayHit))
        {
            isGrounded = true;
            ratMovementAnimator.SetBool("isGrounded", isGrounded);
            isClambering = false;
            //hasClambered = false;
        }
        //accursed slope logic
        else if ((rb.velocity.y > 0.01 || rb.velocity.y < -0.01) && !chargingJump && GetGroundedRaycast(ref groundRayHit))
        {
            isGrounded = true;
            ratMovementAnimator.SetBool("isGrounded", isGrounded);
            isClambering = false;
            //hasClambered = false;
        }
        /*
        // if moving in the vertical direction and raycast is not touching the floor
        else if ((rb.velocity.y > 0.01 || rb.velocity.y < -0.01) && !GetGroundedRaycast())
        {
            isGrounded = false;
        }
        */


        /*  
         *  Jump Charging  
         */

        //Jump Disabled Timer (If jump is canceled, disable it for 0.5 seconds)
        if (!jumpEnabled)
        {
            jumpCDTimer += Time.deltaTime;

            if (jumpCDTimer >= jumpCDTimerFull)
            {
                jumpCDTimer = 0;
                jumpEnabled = true;
            }
        }
        //check for grounded Disabled Timer (After a jump, disable this detection for a second (curse you slopes))
        if (!groundCheckEnabled)
        {
            groundCheckTimer += Time.deltaTime;

            if (groundCheckTimer >= groundCheckTimerFull)
            {
                groundCheckTimer = 0;
                groundCheckEnabled = true;
                //Debug.Log("Check for ground again");
            }
        }

        if (jumpEnabled && jumpAction.ReadValue<float>() == 1f && isGrounded && groundRayHit.normal.y >= 0.75)
        {

            if (!chargingJump)
            {
                chargingJump = true;
                ratMovementAnimator.SetBool("chargingJump", chargingJump);
            }
            alteredHJumpForce += .2f;
            alteredVJumpForce -= .1f;

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


        /*  
         *  Jump Itself  
         */
        if (jumpAction.ReadValue<float>() == 0f && chargingJump && touchingWall && Mathf.Abs(groundRayHit.normal.y) >= 0.98)
        {
            isGrounded = false; // Player is no longer grounded after jumping
            ratMovementAnimator.SetBool("isGrounded", isGrounded);
            // Apply an upward force to the Rigidbody
            Clamber();
            chargingJump = false;
            ratMovementAnimator.SetBool("chargingJump", chargingJump);


            alteredHJumpForce = beginHJumpForce;
            alteredVJumpForce = beginVJumpForce;
            jumpChargeBar.fillAmount = 0f;
        }
        else if (jumpAction.ReadValue<float>() == 0f && chargingJump)
        {
            isGrounded = false; // Player is no longer grounded after jumping
            ratMovementAnimator.SetBool("isGrounded", isGrounded);
            groundCheckEnabled = false;
            // Apply an upward force to the Rigidbody
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * alteredVJumpForce, ForceMode.Impulse);
            rb.AddForce(wallCollisionObject.transform.forward * alteredHJumpForce, ForceMode.Impulse);
            chargingJump = false;
            ratMovementAnimator.SetBool("chargingJump", chargingJump);
            ratMovementAnimator.SetTrigger("Jump");

            alteredHJumpForce = beginHJumpForce;
            alteredVJumpForce = beginVJumpForce;
            jumpChargeBar.fillAmount = 0f;
        }
        //Jump Cancelling
        //cancel if no longer grounded
        else if (chargingJump && !isGrounded)
        {
            chargingJump = false;
            ratMovementAnimator.SetBool("chargingJump", chargingJump);
            alteredHJumpForce = beginHJumpForce;
            alteredVJumpForce = beginVJumpForce;
            jumpChargeBar.fillAmount = 0f;
            jumpEnabled = false;
        }
        else if (cancelJumpAction.ReadValue<float>() == 1f && chargingJump)
        {
            chargingJump = false;
            ratMovementAnimator.SetBool("chargingJump", chargingJump);
            alteredHJumpForce = beginHJumpForce;
            alteredVJumpForce = beginVJumpForce;
            jumpChargeBar.fillAmount = 0f;
            jumpEnabled = false;
        }


        /*
         *  Wall Clamber 
         */

        
        /*      Disabling main clamber function for now
        //if wall detection and jump and not grounded, clamber shenanigans :)
        if (jumpAction.ReadValue<float>() == 1f && touchingWall && !isGrounded && !hasClambered)
        {
            Clamber();

        }
        */
        if (isClambering && !touchingWall && !isGrounded && rb.velocity.y > 0)
        {

            //Debug.Log("Go forwards");

            rb.velocity = Vector3.zero;
            rb.AddForce(wallCollisionObject.transform.forward * 10f, ForceMode.Impulse);

            isClambering = false;
        }


        /*
         *  Parameters for debugging. 
         *  Comment out if not needed
         */

        //Debug.Log("IsClamber: " + isClambering);
        //Debug.Log("HasClamber: " + hasClambered);
        //Debug.Log("Grounded: " + isGrounded);
        //Debug.Log("TouchWall: " + touchingWall);
        //Debug.Log(groundRayHit.normal);
        Debug.Log(currentMovementDir);
        Debug.Log(moveDir);
        //Debug.Log(ratMovementAnimator.GetCurrentAnimatorClipInfo(0)[0].clip);

        //Raycast Debug lines :)

        Debug.DrawLine(wallCollisionObject.transform.position + (wallCollisionObject.transform.forward * rayCastDistance), wallCollisionObject.transform.position + (wallCollisionObject.transform.forward * rayCastDistance) + new Vector3(0, -1f * groundedRayLength, 0), Color.blue); //Forward
        Debug.DrawLine(wallCollisionObject.transform.position - (wallCollisionObject.transform.forward * rayCastDistance), wallCollisionObject.transform.position - (wallCollisionObject.transform.forward * rayCastDistance) + new Vector3(0, -1f * groundedRayLength, 0), Color.blue); //Backward
        Debug.DrawLine(wallCollisionObject.transform.position + (wallCollisionObject.transform.right * rayCastDistance), wallCollisionObject.transform.position + (wallCollisionObject.transform.right * rayCastDistance) + new Vector3(0, -1f * groundedRayLength, 0), Color.blue);     //Right
        Debug.DrawLine(wallCollisionObject.transform.position - (wallCollisionObject.transform.right * rayCastDistance), wallCollisionObject.transform.position - (wallCollisionObject.transform.right * rayCastDistance) + new Vector3(0, -1f * groundedRayLength, 0), Color.blue);     //Left
        //DIagonal Raycast Debug Lines
        Debug.DrawLine(wallCollisionObject.transform.position + (wallCollisionObject.transform.forward * diagRayCastDistance) + (wallCollisionObject.transform.right * diagRayCastDistance), wallCollisionObject.transform.position + (wallCollisionObject.transform.forward * diagRayCastDistance) + (wallCollisionObject.transform.right * diagRayCastDistance) + new Vector3(0, -1f * groundedRayLength, 0), Color.blue); //Forward Right
        Debug.DrawLine(wallCollisionObject.transform.position + (wallCollisionObject.transform.forward * diagRayCastDistance) - (wallCollisionObject.transform.right * diagRayCastDistance), wallCollisionObject.transform.position + (wallCollisionObject.transform.forward * diagRayCastDistance) - (wallCollisionObject.transform.right * diagRayCastDistance) + new Vector3(0, -1f * groundedRayLength, 0), Color.blue); //Forward Left
        Debug.DrawLine(wallCollisionObject.transform.position - (wallCollisionObject.transform.forward * diagRayCastDistance) + (wallCollisionObject.transform.right * diagRayCastDistance), wallCollisionObject.transform.position - (wallCollisionObject.transform.forward * diagRayCastDistance) + (wallCollisionObject.transform.right * diagRayCastDistance) + new Vector3(0, -1f * groundedRayLength, 0), Color.blue); //Backwards Right
        Debug.DrawLine(wallCollisionObject.transform.position - (wallCollisionObject.transform.forward * diagRayCastDistance) - (wallCollisionObject.transform.right * diagRayCastDistance), wallCollisionObject.transform.position - (wallCollisionObject.transform.forward * diagRayCastDistance) - (wallCollisionObject.transform.right * diagRayCastDistance) + new Vector3(0, -1f * groundedRayLength, 0), Color.blue); //Backwards Left


        /*Update Animation State*/
        //UpdateAnimator(); This is currently spread throughout the script in pieces rather than at the end like this

        //Footsetp Audio

        if (currentMovementDir != Vector3.zero && lockMoveAction.ReadValue<float>() == 0f && isGrounded && !chargingJump)
        {
            if (!audioManager.IsPlayingSFX())
            {
                audioManager.Play("Footsteps");
            }
        }
        else
        {
            if (audioManager.IsPlayingSFX())
            {
                audioManager.StopSFX();
            }
        }

    }

    private void UpdateAnimator()
    {
        
        if (currentMovementDir == new Vector3(0f, 0f, 1.0f)) //Backwards
        {
            moveDir = ratFacingDirection.BACKWARDS;
        }
        else if (currentMovementDir == new Vector3(0.71f, 0f, 0.71f))
        {
            moveDir = ratFacingDirection.BACKWARDS_RIGHT;
        }
        else if (currentMovementDir == new Vector3(1.0f, 0f, 0f))
        {
            moveDir = ratFacingDirection.RIGHT;
        }
        else if (currentMovementDir == new Vector3(0.71f, 0f, -0.71f))
        {
            moveDir = ratFacingDirection.FORWARDS_RIGHT;
        }
        else if (currentMovementDir == new Vector3(0f, 0f, -1.0f))
        {
            moveDir = ratFacingDirection.FORWARDS;
        }
        else if (currentMovementDir == new Vector3(-0.71f, 0f, -0.71f))
        {
            moveDir = ratFacingDirection.FORWARDS_LEFT;
        }
        else if (currentMovementDir == new Vector3(-1.0f, 0f, 0f))
        {
            moveDir = ratFacingDirection.LEFT;
        }
        else if (currentMovementDir == new Vector3(-0.71f, 0f, 0.71f))
        {
            moveDir = ratFacingDirection.BACKWARDS_LEFT;
        }


        ratMovementAnimator.SetInteger("MoveDir", (int)moveDir);
        ratMovementAnimator.SetFloat("MovementX", currentMovementDir.x);
        ratMovementAnimator.SetFloat("MovementZ", currentMovementDir.z);
        ratMovementAnimator.SetBool("isGrounded", isGrounded);
        ratMovementAnimator.SetBool("chargingJump", chargingJump);
    }


    private void Clamber()
    {
        rb.AddForce(Vector3.up * clamberForce, ForceMode.Impulse);

        //hasClambered = true;
        isClambering = true;
        ratMovementAnimator.SetTrigger("Jump");
    }

    //TriggerEnter & Exit for WallTouching Collision

    private void OnTriggerEnter(Collider other)
    {
        if (touchingWall == false)
        {
            touchingWall = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        touchingWall = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (touchingWall == true)
        {
            touchingWall = false;
        }
    }
    private bool GetGroundedRaycast(ref RaycastHit hit)
    {        

        //Many Raycasts :)
        
        if (((groundCheckEnabled &&
            Physics.Raycast(wallCollisionObject.transform.position + (wallCollisionObject.transform.forward * rayCastDistance), Vector3.down, out hit, groundedRayLength, groundRayMask) || //Forward Raycast
            Physics.Raycast(wallCollisionObject.transform.position + (wallCollisionObject.transform.forward * diagRayCastDistance) - (wallCollisionObject.transform.right * diagRayCastDistance), Vector3.down, out hit, groundedRayLength, groundRayMask) || //Forward-Left Raycast
            Physics.Raycast(wallCollisionObject.transform.position + (wallCollisionObject.transform.forward * diagRayCastDistance) + (wallCollisionObject.transform.right * diagRayCastDistance), Vector3.down, out hit, groundedRayLength, groundRayMask) || //Forward-Right Raycast
            Physics.Raycast(wallCollisionObject.transform.position - (wallCollisionObject.transform.forward * rayCastDistance), Vector3.down, out hit, groundedRayLength, groundRayMask)) ||  //Backward Raycast
            Physics.Raycast(wallCollisionObject.transform.position - (wallCollisionObject.transform.right * rayCastDistance), Vector3.down, out hit, groundedRayLength, groundRayMask)    ||  //Left Raycast
            Physics.Raycast(wallCollisionObject.transform.position + (wallCollisionObject.transform.right * rayCastDistance), Vector3.down, out hit, groundedRayLength, groundRayMask)))      //Right Raycast
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /* Audio Management */
    /*
    //From Ana's Script: Starts Footstep loop
    void StartFootsteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootstep), 0f, footstepSpeed);
        //audioManager.Play("Footsteps");
    }
    //From Ana's Script: End Footstep loop
    void StopFootsteps()
    {
        playingFootsteps = false;
        audioManager.Stop();
        CancelInvoke(nameof(PlayFootstep));
    }
    //From Ana's Script: Plays footsteps audio
    void PlayFootstep()
    {
        audioManager.Play("Footsteps");
    }
    */
}
