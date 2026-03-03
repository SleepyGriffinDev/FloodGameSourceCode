using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossyRatMovementBehaviour : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            //animator.SetTrigger("hop");
            //float zDifference = 0;
            //if (transform.position.z % 1 !=0)
            {
                //zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }
            transform.position = (transform.position + new Vector3(1, 0, 0));
        }

        else if (Input.GetKeyDown(KeyCode.A))
        {
            //animator.SetTrigger("hop");
            transform.position = (transform.position + new Vector3(0, 0, 1));
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            //animator.SetTrigger("hop");
            transform.position = (transform.position + new Vector3(0, 0, -1));
        }
    }
}
