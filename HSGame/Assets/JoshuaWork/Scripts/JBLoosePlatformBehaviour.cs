using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JBLoosePlatformBehaviour : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    BoxCollider BoxCollider;

    bool beginDescent = false;

    float timer = 0f;

    void Start()
    {
        rb.freezeRotation = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.down * 0.1f;
            beginDescent = true;
        }
    }

    void FixedUpdate()
    {
        if (beginDescent)
        {
            timer += Time.deltaTime;
        }

        if (timer > 1f)
        {
            rb.freezeRotation = false;
            rb.AddForce(Vector3.down * 20);
            BoxCollider.enabled = false;
            rb.useGravity = true;
        }
    }
}
