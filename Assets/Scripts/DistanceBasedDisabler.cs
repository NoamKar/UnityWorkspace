using System.Collections.Generic;
using UnityEngine;

public class DistanceBasedDisabler : MonoBehaviour
{
    public Transform cameraTransform;           // Reference to the camera
    public float disableDistance = 50f;         // Distance threshold for disabling objects
    public float checkInterval = 1f;            // Interval (in seconds) to check the distance
    public List<GameObject> parentObjects = new List<GameObject>(); // List of parent objects
    public string targetTag = "";               // Tag for automatic detection (optional)

    private List<GameObject> objectsToMonitor = new List<GameObject>();
    private Dictionary<GameObject, bool> objectStates = new Dictionary<GameObject, bool>();

    void Start()
    {
        // If no camera is set, use the main camera
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Populate objectsToMonitor list automatically
        foreach (GameObject parent in parentObjects)
        {
            if (parent != null)
            {
                // Add all children of each parent object
                AddChildObjects(parent);
            }
        }

        if (!string.IsNullOrEmpty(targetTag))
        {
            // Add all objects with the specified tag
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
            objectsToMonitor.AddRange(taggedObjects);
        }

        // Initialize the dictionary with the active state of each object
        foreach (GameObject obj in objectsToMonitor)
        {
            objectStates[obj] = obj.activeSelf;
        }

        // Start the distance check coroutine
        StartCoroutine(CheckDistances());
    }

    private void AddChildObjects(GameObject parent)
    {
        // Recursively add all child objects of the parent
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject != parent) // Skip the parent itself
            {
                objectsToMonitor.Add(child.gameObject);
            }
        }
    }

    private System.Collections.IEnumerator CheckDistances()
    {
        while (true)
        {
            foreach (GameObject obj in objectsToMonitor)
            {
                float distance = Vector3.Distance(cameraTransform.position, obj.transform.position);

                // Disable objects beyond disableDistance; re-enable when they come within range
                if (distance > disableDistance && objectStates[obj])
                {
                    obj.SetActive(false);
                    objectStates[obj] = false; // Update state

                    // Log debug message
                    Debug.Log($"Object '{obj.name}' has been disabled due to distance.");
                }
                else if (distance <= disableDistance && !objectStates[obj])
                {
                    obj.SetActive(true);
                    objectStates[obj] = true; // Update state

                    // Log debug message
                    Debug.Log($"Object '{obj.name}' has been re-enabled as it is within range.");
                }
            }

            // Wait for the next check interval
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (cameraTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(cameraTransform.position, disableDistance);
        }
    }
}
