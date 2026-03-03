using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EdibleObjectBehaviour : MonoBehaviour
{
    [SerializeField]
    int health = 10;

    //[SerializeField]
    //Animator animator;

    [SerializeField]
    UnityEvent OnNibbled;

    public void TakeDamage(int howMuch)
    {
        health -= howMuch;
        //animator.SetTrigger("TakeDamage");
        if (health <= 0)
        {
            OnNibbled.Invoke();
            Destroy(this.gameObject);
            Debug.Log("Object destroyed");
        }
    }
}
