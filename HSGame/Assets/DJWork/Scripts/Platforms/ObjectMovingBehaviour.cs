using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovingBehaviour : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 0.2f;
    [SerializeField]
    float timer = 0;
    [SerializeField] 
    Rigidbody player;



    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, 0, moveSpeed * Time.deltaTime); // moves the object down

        timer += Time.deltaTime;

        if (timer >= 33)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player")) // Ensure your player object has the "Player" tag
        {
            // Parent the player to this platform
            player.velocity -= new Vector3(player.position.x, player.position.y, moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the exiting object is the player
        if (!collision.gameObject.tag.Equals("Player")) // Ensure your player object has the "Player" tag
        {
            // Parent the player to this platform
            player.velocity = Vector3.zero;
        }
    }
}
