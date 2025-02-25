using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderNew : MonoBehaviour
{
    private AsyncOperation sceneLoadOperation;

    // Public method to start preloading a scene in the background
    public void PreloadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Preloading scene: " + sceneName);
            StartCoroutine(PreloadSceneCoroutine(sceneName));
        }
        else
        {
            Debug.LogError("Scene name is empty. Please provide a valid scene name.");
        }
    }

    // Coroutine to load the scene asynchronously
    private IEnumerator PreloadSceneCoroutine(string sceneName)
    {
        sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);
        sceneLoadOperation.allowSceneActivation = false; // Do not switch yet

        while (sceneLoadOperation.progress < 0.9f)
        {
            yield return null; // Wait until the scene is almost fully loaded
        }

        Debug.Log("Scene preloaded and ready.");
    }

    // Method to change scene instantly after preloading
    public void ChangeScene()
    {
        if (sceneLoadOperation != null)
        {
            Debug.Log("Switching to preloaded scene...");
            sceneLoadOperation.allowSceneActivation = true; // Instantly switch scenes
        }
        else
        {
            Debug.LogError("No preloaded scene found. Call PreloadScene first.");
        }
    }


}
