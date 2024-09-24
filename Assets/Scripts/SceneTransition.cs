using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName = "OutdoorScene";  // Name of the next scene to load
    public string currentSceneName = "IndoorScene"; // The name of the current scene to unload
    public Transform transitionPoint;   // The position where the scene transition occurs
    public Transform cameraOffset;      // The object to track (which could be the camera offset directly)
    public float loadSceneAheadTime = 5f;          // Time to load the scene ahead of the actual scene switch
    public float unloadDelayAfterTransition = 2f;  // Time to unload the current scene after transition

    private bool isSceneLoaded = false;
    private AsyncOperation asyncLoad;

    void Start()
    {
        // Preload the next scene after the defined delay
        StartCoroutine(PreloadOutdoorSceneWithDelay(loadSceneAheadTime));
    }

    void Update()
    {
        if (transitionPoint == null || cameraOffset == null)
        {
            Debug.LogError("Transition point or camera offset is not assigned.");
            return;
        }

        // Check if the cameraOffset has passed the transition point
        if (Vector3.Distance(cameraOffset.position, transitionPoint.position) < 0.1f && asyncLoad != null && !asyncLoad.allowSceneActivation)
        {
            Debug.Log("Starting scene transition to " + nextSceneName);
            asyncLoad.allowSceneActivation = true; // Allow the new scene to activate
            StartCoroutine(SetNextSceneObjectPosition());
            StartCoroutine(UnloadCurrentSceneWithDelay(unloadDelayAfterTransition)); // Unload with a delay
        }
    }

    IEnumerator PreloadOutdoorSceneWithDelay(float delay)
    {
        Debug.Log("Preloading scene " + nextSceneName + " in " + delay + " seconds.");
        yield return new WaitForSeconds(delay);

        // Start loading the scene in the background but do not activate it immediately
        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log(nextSceneName + " is preloaded and ready for activation.");
                break;
            }
            yield return null;
        }
    }

    IEnumerator SetNextSceneObjectPosition()
    {
        yield return null; // Allow the scene to activate first
        Scene nextScene = SceneManager.GetSceneByName(nextSceneName);

        // Ensure the scene is loaded and activated
        if (nextScene.isLoaded)
        {
            Debug.Log("Scene " + nextSceneName + " loaded successfully.");

            // Find the camera offset in the next scene and apply the exact position
            GameObject[] rootObjects = nextScene.GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                if (obj.CompareTag(cameraOffset.tag))
                {
                    // Set the position and rotation using the current camera offset in the new scene
                    obj.transform.position = cameraOffset.position;
                    obj.transform.rotation = cameraOffset.rotation; // Keep the rotation consistent
                    Debug.Log(cameraOffset.name + " position transferred to the next scene.");
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Failed to load the next scene: " + nextSceneName);
        }
    }

    IEnumerator UnloadCurrentSceneWithDelay(float delay)
    {
        Debug.Log("Preparing to unload scene " + currentSceneName + " in " + delay + " seconds.");
        yield return new WaitForSeconds(delay);

        // Check if the scene is valid and currently loaded
        Scene currentScene = SceneManager.GetSceneByName(currentSceneName);
        if (currentScene.isLoaded)
        {
            Debug.Log("Unloading current scene: " + currentSceneName);
            SceneManager.UnloadSceneAsync(currentSceneName);
        }
        else
        {
            Debug.LogError("Scene to unload is invalid or not loaded: " + currentSceneName);
        }
    }
}
