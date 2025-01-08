using UnityEngine;
using UnityEngine.Events;

public class GazeInOutInteraction : MonoBehaviour
{
    public GameObject raySource;                // The object emitting the ray (e.g., Main Camera)
    public float rayLength = 15f;                // Length of the ray
    public LayerMask layerMaskInteract;          // Layers to interact with

    private bool isGazed = false;                // Track if the object is being gazed at

    public UnityEvent GazeEnter;                 // Event for when gaze enters
    public UnityEvent GazeExit;                  // Event for when gaze exits

    private void Update()
    {
        RaycastHit hit;
        Vector3 direction = raySource.transform.forward;

        Debug.DrawRay(raySource.transform.position, direction * rayLength, Color.red);

        // Perform the raycast
        if (Physics.Raycast(raySource.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Gaze has entered the object
                if (!isGazed)
                {
                    isGazed = true;
                    Debug.Log("Gaze Enter: " + gameObject.name);
                    GazeEnter.Invoke();  // Trigger gaze enter event
                }
            }
            else
            {
                // Ray hit a different object
                if (isGazed)
                {
                    HandleGazeExit();  // Handle gaze exit
                }
            }
        }
        else
        {
            // Ray hit nothing (empty space)
            if (isGazed)
            {
                HandleGazeExit();  // Handle gaze exit
            }
        }
    }

    private void HandleGazeExit()
    {
        Debug.Log("Gaze Exit: " + gameObject.name);
        isGazed = false;  // Reset the gaze state
        GazeExit.Invoke();  // Trigger the gaze exit event
    }
}
