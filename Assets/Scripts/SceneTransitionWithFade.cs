using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionWithFade : MonoBehaviour
{
    public static SceneTransitionWithFade instance; // Singleton instance

    public Image fadeImage; // Assign a full-screen UI Image in the Canvas
    public float fadeDuration = 2f;
    public string nextSceneName = "Scene01"; // Change to your scene name

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
    }

    private void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            SetImageAlpha(1f);
        }

        StartCoroutine(PlayOpeningAndTransition());
    }

    private IEnumerator PlayOpeningAndTransition()
    {
        yield return new WaitForSeconds(1f); // Adjust for credits duration

        yield return StartCoroutine(FadeToBlack());

        SceneManager.LoadScene(nextSceneName);
        yield return null; // Wait for scene load

        yield return StartCoroutine(FadeFromBlack());

        DestroyCanvasAndSelf(); // Destroy transition manager and canvas
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            SetImageAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetImageAlpha(1f);
    }

    private IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetImageAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetImageAlpha(0f);
        fadeImage.gameObject.SetActive(false);
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

    private void DestroyCanvasAndSelf()
    {
        Debug.Log("[SceneTransition] Destroying transition UI...");

        // Try to destroy the Canvas parent
        if (fadeImage != null && fadeImage.transform.parent != null)
        {
            Destroy(fadeImage.transform.parent.gameObject); // Destroy the entire Canvas
        }

        // Destroy this transition object
        Destroy(gameObject);
    }
}
