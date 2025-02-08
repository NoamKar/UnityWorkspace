using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFaderVideo : MonoBehaviour
{
    public RawImage targetImage;  // Assign your Raw Image in the Inspector
    public float fadeDuration = 1f; // Duration of the fade
    public bool fadeInOnStart = false; // Should it fade in when the scene starts?

    private void Start()
    {
        if (fadeInOnStart)
        {
            StartFadeIn();
        }
    }

    // Call this to fade in (make visible)
    public void StartFadeIn()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    // Call this to fade out (make invisible)
    public void StartFadeOut()
    {
        StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (targetImage == null)
        {
            Debug.LogError("UIFader: No Raw Image assigned.");
            yield break;
        }

        Color imageColor = targetImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            imageColor.a = Mathf.Lerp(startAlpha, endAlpha, t);
            targetImage.color = imageColor;
            yield return null;
        }

        // Ensure final value is set
        imageColor.a = endAlpha;
        targetImage.color = imageColor;
    }
}
