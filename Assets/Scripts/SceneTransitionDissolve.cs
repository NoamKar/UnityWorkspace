using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionDissolve : MonoBehaviour
{
    public static SceneTransitionDissolve instance;  // Singleton instance
    public Image fadeImage;                         // Fullscreen UI Image for fading
    public float fadeDuration = 2f;                 // Duration of the fade
    public string nextSceneName;                    // Name of the next scene to load

    private AsyncOperation sceneLoadOperation;
    private bool isFirstScene = true;               // Flag to skip fade-in for the first scene

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Make this object persistent
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
            return;
        }

        if (fadeImage == null)
        {
            Debug.LogWarning("Fade Image is not assigned. Will attempt to find it in the scene.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  // Add listener for when a new scene is loaded
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Remove listener to avoid memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeImage == null)
        {
            fadeImage = FindObjectOfType<Image>();  // Dynamically find the UI Image after scene load
            if (fadeImage != null)
            {
                Debug.Log("Fade Image found and assigned after scene load.");
            }
            else
            {
                Debug.LogError("No UI Image found in the scene.");
                return;
            }
        }

        if (isFirstScene)
        {
            // Skip the fade-in for the first scene
            SetImageAlpha(fadeImage, 0f);  // Make the image fully transparent
            isFirstScene = false;          // Ensure future scenes do not skip fade-in
        }
        else
        {
            // Automatically fade in for subsequent scenes
            SetImageAlpha(fadeImage, 1f);  // Start fully opaque
            StartCoroutine(FadeIn());      // Fade in the new scene
        }
    }

    public void StartSceneTransition()
    {
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        yield return StartCoroutine(PreloadNextScene());

        yield return StartCoroutine(FadeOut());

        sceneLoadOperation.allowSceneActivation = true;

        yield return new WaitForSeconds(0.1f);  // Allow the next scene to load
    }

    private IEnumerator PreloadNextScene()
    {
        sceneLoadOperation = SceneManager.LoadSceneAsync(nextSceneName);
        sceneLoadOperation.allowSceneActivation = false;  // Preload without activation
        while (sceneLoadOperation.progress < 0.9f)
        {
            yield return null;
        }
        Debug.Log("Next scene preloaded.");
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            SetImageAlpha(fadeImage, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetImageAlpha(fadeImage, 1f);  // Fully opaque
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetImageAlpha(fadeImage, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetImageAlpha(fadeImage, 0f);  // Fully transparent
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
