using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlatformBehavior : MonoBehaviour
{
    [SerializeField]
    string playerTag = "Player";
    [SerializeField]
    Transform platform;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the entering object is the player
        if (collision.gameObject.tag.Equals(playerTag)) // Ensure your player object has the "Player" tag
        {
            // Parent the player to this platform
            collision.gameObject.transform.parent = platform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        // Check if the exiting object is the player
        if (collision.gameObject.tag.Equals(playerTag)) // Ensure your player object has the "Player" tag
        {
            // Unparent the player
            collision.gameObject.transform.parent = null;
        }
    }
}
