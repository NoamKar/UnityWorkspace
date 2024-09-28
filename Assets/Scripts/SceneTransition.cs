using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName = "Scene05_001";  // Name of the next scene to load
    public string currentSceneName = "Scene03+04_019"; // The name of the current scene to unload
    public GameObject referenceObject;  // The object to track (e.g., cameraOffset)
    public Vector3 transitionPosition;  // Position where the scene transition should occur
    public float loadSceneAheadTime = 5f;  // Time to load the scene ahead of the actual scene switch
    public float unloadDelayAfterTransition = 2f;  // Time to unload the current scene after transition
    public bool triggerOnXAxis = true;  // Set this to true to trigger the transition on X axis movement
    public bool triggerOnYAxis = false; // Set this to true to trigger on Y axis movement
    public bool triggerOnZAxis = false; // Set this to true to trigger on Z axis movement

    private bool isSceneLoaded = false;
    private bool hasTriggeredTransition = false; // To ensure the transition happens only once
    private AsyncOperation asyncLoad;

    void Start()
    {
        // Preload the next scene after the defined delay
        StartCoroutine(PreloadOutdoorSceneWithDelay(loadSceneAheadTime));
    }

    void Update()
    {
        if (referenceObject == null)
        {
            Debug.LogError("Reference object is not assigned.");
            return;
        }

        // Check if the referenceObject has moved past the transition position on any specified axis
        if (!hasTriggeredTransition && HasPassedTransitionPosition())
        {
            Debug.Log("Transition position reached, preparing to start the scene transition.");
            hasTriggeredTransition = true;  // Trigger the transition only once
            StartCoroutine(ActivateNextScene());
        }
    }

    bool HasPassedTransitionPosition()
    {
        Vector3 refPos = referenceObject.transform.position;

        // Check if the reference object has moved past the transition position on the desired axis
        if (triggerOnXAxis && refPos.x >= transitionPosition.x) return true;
        if (triggerOnYAxis && refPos.y >= transitionPosition.y) return true;
        if (triggerOnZAxis && refPos.z >= transitionPosition.z) return true;

        return false;
    }

    IEnumerator PreloadOutdoorSceneWithDelay(float delay)
    {
        Debug.Log("Preloading scene " + nextSceneName + " in " + delay + " seconds.");
        yield return new WaitForSeconds(delay);

        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading scene progress: " + asyncLoad.progress);
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("Scene preloaded, waiting for activation.");
                break;
            }
            yield return null;
        }
    }

    IEnumerator ActivateNextScene()
    {
        // Wait until the scene has been fully loaded before proceeding
        if (asyncLoad == null || asyncLoad.progress < 0.9f)
        {
            Debug.LogError("Attempting to activate scene before it's fully loaded.");
            yield break;
        }

        asyncLoad.allowSceneActivation = true;

        // Wait a frame to ensure scene activation is complete
        yield return new WaitForSeconds(1f);

        Debug.Log("Scene activated, waiting to unload current scene.");

        // Unload the current scene after a delay
        yield return new WaitForSeconds(unloadDelayAfterTransition);
        UnloadCurrentScene();
    }

    void UnloadCurrentScene()
    {
        // Check if the current scene is valid and loaded
        Scene currentScene = SceneManager.GetSceneByName(currentSceneName);
        if (currentScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(currentSceneName);
            Debug.Log("Unloading current scene: " + currentSceneName);
        }
        else
        {
            Debug.LogError("Scene to unload is invalid or not loaded: " + currentSceneName);
        }
    }
}
