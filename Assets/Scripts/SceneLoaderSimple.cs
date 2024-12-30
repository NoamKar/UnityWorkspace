using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSimple : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName = "OutdoorScene";        // Name of the next scene to load
    public string currentSceneName = "IndoorScene";      // Name of the current scene to unload

    private AsyncOperation asyncLoad;
    private bool isScenePreloaded = false;

    // Public method to start preloading the next scene
    public void PreloadNextScene()
    {
        if (!isScenePreloaded)
        {
            StartCoroutine(PreloadSceneCoroutine());
        }
        else
        {
            Debug.LogWarning("Scene has already been preloaded.");
        }
    }

    // Public method to activate the preloaded scene and unload the current one
    public void ActivateNextScene()
    {
        if (isScenePreloaded)
        {
            asyncLoad.allowSceneActivation = true;
            Debug.Log("Activating next scene: " + nextSceneName);

            // Unload the current scene after one frame
            StartCoroutine(UnloadCurrentScene());
        }
        else
        {
            Debug.LogError("Next scene is not preloaded yet.");
        }
    }

    // Coroutine to handle the scene preloading process
    IEnumerator PreloadSceneCoroutine()
    {
        Debug.Log("Preloading next scene: " + nextSceneName);

        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is fully loaded (but not activated)
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        Debug.Log("Next scene preloaded and ready for activation.");
        isScenePreloaded = true;
    }

    // Coroutine to unload the current scene
    IEnumerator UnloadCurrentScene()
    {
        yield return null;  // Wait for the current frame to finish
        SceneManager.UnloadSceneAsync(currentSceneName);
        Debug.Log("Unloading current scene: " + currentSceneName);
    }
}