using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SturdyFloorBehaviourUpdated : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    BoxCollider BoxCollider;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            BoxCollider.enabled = false;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
