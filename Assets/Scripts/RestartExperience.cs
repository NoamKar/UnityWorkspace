using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartExperience : MonoBehaviour
{
    public string firstSceneName = "Scene01"; // Set your first scene name

    public void Restart()
    {
        Debug.Log("[RestartExperience] Restarting experience...");

        // Try to find ExperienceManager in the scene
        ExperienceManager experienceManager = FindObjectOfType<ExperienceManager>();

        if (experienceManager != null)
        {
            experienceManager.RestartExperience();  // Call the existing restart function
        }
        else
        {
            Debug.LogWarning("[RestartExperience] ExperienceManager not found! Forcing full restart.");

            // If ExperienceManager is missing (which shouldn't happen), manually reload everything
            ResetPersistentObjects();
            SceneManager.LoadScene(firstSceneName);
        }
    }

    private void ResetPersistentObjects()
    {
        Debug.Log("[RestartExperience] Resetting all persistent objects...");

        // Find all objects marked as DontDestroyOnLoad
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.buildIndex == -1) // If object is in the DontDestroyOnLoad scene
            {
                Destroy(obj);
            }
        }
    }
}
