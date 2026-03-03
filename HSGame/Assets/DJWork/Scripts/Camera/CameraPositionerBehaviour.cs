using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionerBehaviour : MonoBehaviour
{
    [Header("Assign the parents here")]
    public GameObject parentToEnable;
    public GameObject parentToDisable;

    [Header("Tag of the player object")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag(playerTag))
        {
            if (parentToEnable != null)
                parentToEnable.SetActive(true);

            if (parentToDisable != null)
                parentToDisable.SetActive(false);
        }
    }
}
