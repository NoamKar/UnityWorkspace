using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionDissolve : MonoBehaviour
{
    public static SceneTransitionDissolve instance;
    public Image fadeImage;
    public float fadeDuration = 2f;
    public float finalOpacity = 0.7f;
    public string nextSceneName;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private AsyncOperation sceneLoadOperation;
    private bool isScenePreloaded = false;

    public bool IsScenePreloaded => isScenePreloaded;

    private void Awake()
    {
        // **Allow SceneTransitionDissolve to reset when scene changes**
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PreloadNextScene(string sceneName = null)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            nextSceneName = sceneName;
        }

        if (!isScenePreloaded)
        {
            StartCoroutine(PreloadNextSceneCoroutine());
        }
    }

    public void StartSceneTransition()
    {
        if (!isScenePreloaded || sceneLoadOperation == null)
        {
            Debug.LogWarning("[SceneTransition] Scene not preloaded correctly. Loading manually...");
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator PreloadNextSceneCoroutine()
    {
        Debug.Log($"[SceneTransition] Preloading scene: {nextSceneName}");
        sceneLoadOperation = SceneManager.LoadSceneAsync(nextSceneName);
        sceneLoadOperation.allowSceneActivation = false;

        while (sceneLoadOperation.progress < 0.9f)
        {
            yield return null;
        }

        isScenePreloaded = true;
        Debug.Log($"[SceneTransition] Scene '{nextSceneName}' preloaded.");
    }

    private IEnumerator TransitionCoroutine()
    {
        yield return StartCoroutine(FadeOut());

        if (sceneLoadOperation != null)
        {
            Debug.Log("[SceneTransition] Activating preloaded scene.");
            sceneLoadOperation.allowSceneActivation = true;
            yield return sceneLoadOperation;
        }
        else
        {
            Debug.LogError("[SceneTransition] Scene load operation lost. Reloading manually.");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
            float alpha = Mathf.Lerp(0f, finalOpacity, curveValue);
            SetImageAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

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

    public void ResetPreloadStatus()
    {
        Debug.Log("[SceneTransition] Resetting preload status.");
        isScenePreloaded = false;
        sceneLoadOperation = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset preload status when a new scene is loaded
        ResetPreloadStatus();
    }
}
