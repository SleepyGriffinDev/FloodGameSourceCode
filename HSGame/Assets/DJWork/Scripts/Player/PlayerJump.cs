using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on this GameObject!");
            enabled = false; // Disable the script if no Rigidbody is found
        }
    }

    void Update()
    {
        // Check for jump input and if the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is colliding with the ground
        if (collision.gameObject.CompareTag("Ground")) // Ensure your ground object has the "Ground" tag
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Player is no longer touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Jump()
    {
        // Apply an upward force to the Rigidbody
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // Player is no longer grounded after jumping
    }
}