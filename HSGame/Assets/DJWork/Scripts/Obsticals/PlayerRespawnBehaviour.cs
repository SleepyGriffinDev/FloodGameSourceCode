using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform checkPoint;


    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstical")
        {
            transform.position = targetPosition;
            Debug.Log("Dead!");
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object has the "Player" tag
        if (other.CompareTag("Obstical"))
        {
            // Teleport the player to the target location
            other.transform.position = checkPoint.position;
            Debug.Log("Dead!");
        }
    }
}