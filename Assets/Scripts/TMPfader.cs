using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFader : MonoBehaviour
{
    public TMP_Text textMeshPro;  // Optional: Assign a TMP_Text component
    public Image uiImage;         // Optional: Assign a UI Image component
    public float fadeDuration = 1.5f;  // Adjust fade speed

    private void Start()
    {
        // Optional: Start fully transparent
        SetAlpha(0f);
    }

    public void FadeIn()
    {
        StartCoroutine(FadeUI(0f, 1f));  // Fade from transparent to visible
    }

    public void FadeOut()
    {
        StartCoroutine(FadeUI(1f, 0f));  // Fade from visible to transparent
    }

    private IEnumerator FadeUI(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            // Apply alpha to TextMeshPro if assigned
            if (textMeshPro != null)
            {
                Color textColor = textMeshPro.color;
                textColor.a = alpha;
                textMeshPro.color = textColor;
            }

            // Apply alpha to UI Image if assigned
            if (uiImage != null)
            {
                Color imageColor = uiImage.color;
                imageColor.a = alpha;
                uiImage.color = imageColor;
            }

            yield return null;
        }

        // Ensure final alpha is set
        SetAlpha(endAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (textMeshPro != null)
        {
            Color textColor = textMeshPro.color;
            textColor.a = alpha;
            textMeshPro.color = textColor;
        }

        if (uiImage != null)
        {
            Color imageColor = uiImage.color;
            imageColor.a = alpha;
            uiImage.color = imageColor;
        }
    }
}
