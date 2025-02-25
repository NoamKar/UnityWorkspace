using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionWithFade : MonoBehaviour
{
    public static SceneTransitionWithFade instance;

    public Image fadeImage;
    public float fadeDuration = 2f;
    public string nextSceneName = "Scene01";

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep persistent during transition
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }
    }

    private void Start()
    {
        Debug.Log("[SceneTransition] Starting fade-in...");
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            SetImageAlpha(0f);
        }

        StartCoroutine(TransitionSequence());
    }

    private IEnumerator TransitionSequence()
    {
        yield return new WaitForSeconds(1f); // Adjust for credits duration

        yield return StartCoroutine(FadeToBlack());

        SceneManager.LoadScene(nextSceneName);
        yield return null;

        AssignFadeImage(); // Reassign fadeImage in the new scene
        yield return StartCoroutine(FadeFromBlack());

        // **Remove persistence after transition completes**
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Destroy(gameObject);
    }

    private IEnumerator FadeToBlack()
    {
        Debug.Log("[SceneTransition] Fading to black...");
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            SetImageAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetImageAlpha(1f);
        Debug.Log("[SceneTransition] Fade to black complete.");
    }

    private IEnumerator FadeFromBlack()
    {
        Debug.Log("[SceneTransition] Fading from black...");
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetImageAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetImageAlpha(0f);
        //fadeImage.gameObject.SetActive(false);
        Debug.Log("[SceneTransition] Fade from black complete.");

    }

    private void SetImageAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
        else
        {
            Debug.LogError("[SceneTransition] Fade Image is missing!");
        }
    }

    private void AssignFadeImage()
    {
        // **Find a new fadeImage in the newly loaded scene**
        Image newFadeImage = FindObjectOfType<Image>();
        if (newFadeImage != null)
        {
            fadeImage = newFadeImage;
            Debug.Log("[SceneTransition] Found new fadeImage after scene load.");
        }
        else
        {
            Debug.LogWarning("[SceneTransition] No fadeImage found after scene load.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

            AssignFadeImage(); // Reassign fadeImage after restart
        
    }
}
