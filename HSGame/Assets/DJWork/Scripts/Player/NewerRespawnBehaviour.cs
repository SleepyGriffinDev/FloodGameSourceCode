using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewerRespawnBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform checkPoint;
    [SerializeField]
    Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            transform.position = checkPoint.position;
            rb.velocity = Vector3.zero;
            //Debug.Log("Dead!");
        }
    }
}
