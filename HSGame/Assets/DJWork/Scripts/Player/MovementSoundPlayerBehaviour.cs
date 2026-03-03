using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundPlayerBehaviour : MonoBehaviour
{
    public AudioManager audioManager; // Assign your AudioSource in the Inspector

    void Update()
    {
        // Check if any of the WASD keys are pressed
        bool isKeyPressed = Input.GetKey(KeyCode.W) ||
                            Input.GetKey(KeyCode.A) ||
                            Input.GetKey(KeyCode.S) ||
                            Input.GetKey(KeyCode.D);

        if (isKeyPressed)
        {
            if (!audioManager.IsPlayingSFX())
            {
                audioManager.Play("Footsteps");
            }
        }
        else
        {
            if (audioManager.IsPlayingSFX())
            {
                audioManager.StopSFX();
            }
        }
    }
}
