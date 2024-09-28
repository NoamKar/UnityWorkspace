using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeamlessSceneTransition : MonoBehaviour
{
    [Header("Scene Names")]
    public string nextSceneName;        // Name of the next scene to load
    public string currentSceneName;     // Name of the current scene to unload

    [Header("Transition Timings")]
    public float preloadTime = 5f;                 // Time to load the scene before the actual scene switch
    public float unloadDelayAfterTransition = 2f;  // Time after transition to unload the current scene

    private AsyncOperation asyncLoad;

    void OnEnable()
    {
        // Start the scene transition process
        StartCoroutine(PreloadAndActivateScene());
    }

    IEnumerator PreloadAndActivateScene()
    {
        // Preload the next scene after a short delay
        Debug.Log("Preloading scene: " + nextSceneName);
        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is nearly fully loaded
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("Scene preloaded. Activating...");
                asyncLoad.allowSceneActivation = true; // Activate the scene immediately
                break;
            }
            yield return null;
        }

        // Give it a moment to ensure the scene is activated
        yield return new WaitForSeconds(1f);

        // Unload the current scene after the specified delay
        yield return new WaitForSeconds(unloadDelayAfterTransition);
        Debug.Log("Unloading current scene: " + currentSceneName);
        SceneManager.UnloadSceneAsync(currentSceneName);
    }
}
