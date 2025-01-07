using UnityEngine;
using UnityEngine.UI;

public class FadeOutCanvas : MonoBehaviour
{
    public Image fadeImage;            // The UI Image to fade
    public float fadeDuration = 2f;    // Duration for the fade-out
    private bool isFading = false;     // Track if fading has started

    void Start()
    {
        if (fadeImage == null)
        {
            fadeImage = GetComponent<Image>();  // Automatically find the Image if not set
        }

        if (fadeImage != null)
        {
            // Start the fade-out process
            SetImageAlpha(1f);  // Ensure the image starts fully opaque
            StartFadeOut();
        }
        else
        {
            Debug.LogError("No Image component found. Please assign the fadeImage.");
        }
    }

    public void StartFadeOut()
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private System.Collections.IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);  // Lerp alpha from 1 to 0
            SetImageAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the image is fully transparent at the end
        SetImageAlpha(0f);

        // Disable the game object after the fade-out
        gameObject.SetActive(false);
    }

    private void SetImageAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }
}
