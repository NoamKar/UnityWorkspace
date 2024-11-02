using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionSimple : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName = "OutdoorScene";        // Name of the next scene to load
    public string currentSceneName = "IndoorScene";      // Name of the current scene to unload

    [Header("Transition Settings")]
    public float preloadTime = 5f;                       // Time to load the scene ahead of the actual switch
    public float unloadDelayAfterTransition = 2f;        // Time to unload the current scene after switch

    private bool isSceneLoaded = false;
    private AsyncOperation asyncLoad;

    void Start()
    {
        // Preload the next scene in the background
        StartCoroutine(PreloadNextScene(preloadTime));
    }

    // Public method to activate the next scene
    public void TriggerSceneActivation()
    {
        if (isSceneLoaded)
        {
            StartCoroutine(ActivateNextScene());
        }
        else
        {
            Debug.LogError("Next scene is not fully preloaded yet.");
        }
    }

    IEnumerator PreloadNextScene(float delay)
    {
        Debug.Log("Preloading next scene in " + delay + " seconds.");
        yield return new WaitForSeconds(delay);

        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is fully loaded (but not activated)
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        Debug.Log("Next scene is preloaded and ready for activation.");
        isSceneLoaded = true;
    }

    IEnumerator ActivateNextScene()
    {
        asyncLoad.allowSceneActivation = true;
        Debug.Log("Activating next scene...");

        // Wait one frame to ensure activation completes
        yield return null;

        // Unload the current scene after a delay
        yield return new WaitForSeconds(unloadDelayAfterTransition);
        Debug.Log("Unloading current scene: " + currentSceneName);
        SceneManager.UnloadSceneAsync(currentSceneName);
    }
}
