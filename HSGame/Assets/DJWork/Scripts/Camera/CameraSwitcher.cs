using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Parents with Virtual Cameras")]
    public GameObject parentWithCameraToEnable;
    public GameObject parentWithCameraToDisable;

    [Header("Player Tag")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (parentWithCameraToEnable != null)
                parentWithCameraToEnable.SetActive(true);

            if (parentWithCameraToDisable != null)
                parentWithCameraToDisable.SetActive(false);
        }
    }
}
