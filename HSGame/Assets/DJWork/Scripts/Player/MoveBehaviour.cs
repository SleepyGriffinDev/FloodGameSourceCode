using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
     public float moveSpeed = 5f; // Adjustable movement speed in Unity Inspector
    public float rotationSpeed = 100f; // Adjustable rotation speed in Unity Inspector

    void Update()
    {
        // Get input for vertical movement (W/S or Up/Down arrow keys)
        float verticalInput = Input.GetAxis("Vertical");
        // Get input for horizontal rotation (A/D or Left/Right arrow keys)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate movement direction relative to the player's forward direction
        Vector3 movement = transform.forward * verticalInput * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World); // Move the player

        // Calculate rotation around the Y-axis
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationAmount); // Rotate the player
    }
}
