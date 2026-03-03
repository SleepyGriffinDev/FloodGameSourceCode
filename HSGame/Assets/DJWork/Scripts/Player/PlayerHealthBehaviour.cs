using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour
{
    [SerializeField]
    //int health = 1;
    public string targetTag = "DangerObj";

    // Update is called once per frame
    // The tag of the object that, when collided with, will cause this object to be destroyed.
    public string target = "Enemy";

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the specified tag
        if (collision.gameObject.CompareTag(target))
        {
            // Destroy this GameObject (the one the script is attached to)
            Destroy(gameObject);
        }
    }
}
