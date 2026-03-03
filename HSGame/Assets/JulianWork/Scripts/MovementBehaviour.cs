using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    Vector3 inputVector = new Vector3(1f, 0f, 1f);

    [SerializeField]
    float speed = 5f;

    [SerializeField]
    float rotationSpeed = 10f;

    [SerializeField]
    Rigidbody rb;

    //[SerializeField]
    //Animator animator;

    [SerializeField]
    float jumpforce = 5f;
    void Update()
    {
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        if (Input.GetButtonDown("Jump"))
        {
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.25f))
            {
                Jump();
            }
        }
    }
    void FixedUpdate()
    {
        Vector3 targetVelocity = inputVector * speed;
        Vector3 velocityDelta = targetVelocity - rb.velocity;
        velocityDelta.y = 0f;
        rb.AddForce(velocityDelta, ForceMode.VelocityChange);

        if (targetVelocity.magnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetVelocity);
            Quaternion smoothRoation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed);
            rb.MoveRotation(smoothRoation);
        }
        //animator.SetFloat("Speed", targetVelocity.magnitude);
    }

    void Jump()
    {
        rb.AddForce(0f, jumpforce, 0f, ForceMode.Impulse);
    }
}
