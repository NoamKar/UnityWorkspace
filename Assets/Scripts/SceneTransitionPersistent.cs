using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitionPersistent : MonoBehaviour
{
    private static SceneTransitionPersistent instance;

    void Awake()
    {
        // Ensure only one instance of SceneTransitionPersistent exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object during transitions
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }
    }

    public void MarkTransitionComplete()
    {
        Debug.Log("[SceneTransition] Transition complete, disabling persistence.");
        instance = null; // Allow a new transition manager to be created later
        Destroy(gameObject); // Remove it after transition
    }
}

