using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionDissolve : MonoBehaviour
{
    public static SceneTransitionDissolve instance;  // Singleton instance
    public Image fadeImage;                          // Fullscreen UI Image for fading
    public float fadeDuration = 2f;                  // Duration of the fade
    public float finalOpacity = 0.7f;                // Final opacity level (between 0 and 1)
    public string nextSceneName;                     // Name of the next scene to load
    public AnimationCurve fadeCurve =                // Curve for smoother fade
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private AsyncOperation sceneLoadOperation;       // Async operation for scene loading
    private bool isScenePreloaded = false;           // Track if the scene is preloaded

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
        }
    }

    // **Public method to preload the next scene**
    public void PreloadNextScene(string sceneName = null)
    {
        if (isScenePreloaded)
        {
            Debug.LogWarning("Scene is already preloaded.");
            return;
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            nextSceneName = sceneName;
        }

        StartCoroutine(PreloadNextSceneCoroutine());
    }

    // **Start the scene transition**
    public void StartSceneTransition()
    {
        if (!isScenePreloaded)
        {
            Debug.LogError("Next scene not preloaded. Call PreloadNextScene() before transitioning.");
            return;
        }

        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator PreloadNextSceneCoroutine()
    {
        sceneLoadOperation = SceneManager.LoadSceneAsync(nextSceneName);
        sceneLoadOperation.allowSceneActivation = false;  // Preload without activating
        while (sceneLoadOperation.progress < 0.9f)
        {
            yield return null;
        }
        isScenePreloaded = true;
        Debug.Log($"Scene '{nextSceneName}' preloaded.");
    }

    private IEnumerator TransitionCoroutine()
    {
        yield return StartCoroutine(FadeOut());

        // Activate the next scene after fade out
        sceneLoadOperation.allowSceneActivation = true;

        yield return null;  // Wait one frame
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);  // Smooth fade using curve
            float alpha = Mathf.Lerp(0f, finalOpacity, curveValue);
            SetImageAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final opacity is applied
        SetImageAlpha(finalOpacity);
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
