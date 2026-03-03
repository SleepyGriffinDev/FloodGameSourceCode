using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFadeBehaviour : MonoBehaviour
{
    [SerializeField]
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            StartCoroutine(FadeToBlack());
        }
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;
            fadeImage.color = Color.Lerp(startColor, targetColor, progress);
            yield return null;
        }

        fadeImage.color = targetColor;
    }
}
