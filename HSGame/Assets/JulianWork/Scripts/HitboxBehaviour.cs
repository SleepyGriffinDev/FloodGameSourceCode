using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxBehaviour : MonoBehaviour
{
    [SerializeField]
    string targetTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            //other.GetComponent<EdibleObjectBehaviour>().TakeDamage(damageAmount);
        }
    }
}
