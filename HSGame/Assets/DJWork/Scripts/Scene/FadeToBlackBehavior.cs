using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeToBlackBehavior : MonoBehaviour
{
    [SerializeField]
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    private bool loadCredits = false;
    private float loadCreditsTimer = 0.0f;
    private float loadCreditsTimerMax = 3.0f;

    [SerializeField]
    AudioManager audioManager;

    //float elapsedTime = 0f;

    private void Update()
    {
        if (loadCredits)
        {
            loadCreditsTimer += Time.deltaTime;

            if (loadCreditsTimer >= loadCreditsTimerMax)
            {
                SceneManager.LoadScene(2); //Load Credits Scene
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fadeImage.color = new Color(0, 0, 0, 1f);
            audioManager.StopSFX();
            audioManager.StopMusic();
            loadCredits = true;
        }
    }

    /* For fade to black, currently doing a cut to black
    private IEnumerator FadeToBlackCoroutine()
    {
        
        Color startColor = fadeImage.color;
        Color targetColor = new Color(255, 255, 255, 1f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;
            fadeImage.color = Color.Lerp(startColor, targetColor, progress);
            yield return null;
        }

        fadeImage.color = targetColor;
    }
    */
}
