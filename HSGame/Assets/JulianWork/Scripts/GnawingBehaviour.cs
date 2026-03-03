using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GnawingBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject attackHitbox;

    [SerializeField]
    MovementBehaviour movement;

    [SerializeField]
    InputAction gnawAction;

    //[SerializeField]
    //Animator animator;

    void OnEnable()
    {
        gnawAction.Enable();
    }

    void OnDisable()
    {
        gnawAction.Disable();
    }

    void Update()
    {
        if (gnawAction.ReadValue<float>() == 1f)
        {
            Gnaw();
        }
    }
    void Gnaw()
    {
        //animator.SetTrigger("Gnaw");
        Invoke("DisableMovement", 0f);
        Invoke("TurnOnHitbox", 0.1f);
        Invoke("TurnOffHitbox", 0.5f);
        Invoke("EnableMovement", 0.4f);
    }

    void TurnOnHitbox()
    {
        attackHitbox.SetActive(true);
    }

    void TurnOffHitbox()
    {
        attackHitbox.SetActive(false);
    }

    void EnableMovement()
    {
       movement.enabled = true;
    }

    void DisableMovement()
    {
       movement.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);

    }
}
