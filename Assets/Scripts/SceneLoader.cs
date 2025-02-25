using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Public method to load a scene by name
    public void ChangeScene(string sceneName)
    {
        // Check if the scene name is not empty
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Changing to scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is empty. Please provide a valid scene name.");
        }
    }

    // Public method to reload the current scene
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Reloading scene: " + currentSceneName);
        SceneManager.LoadScene(currentSceneName);
    }

    // Optional: A method to quit the application (useful for builds)
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}