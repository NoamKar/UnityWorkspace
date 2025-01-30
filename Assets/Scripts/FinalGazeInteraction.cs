using UnityEngine;
using UnityEngine.Events;

public class FinalGazeInteraction : MonoBehaviour
{
    public GameObject raySource;        // The object emitting the ray (e.g., Main Camera)
    public int rayLength = 15;          // Length of the ray
    public LayerMask layerMaskInteract; // Assign appropriate layers to avoid false hits

    public UnityEvent GazeEnter;        // Event for actions on first gaze enter
    public UnityEvent GazeExit;         // Event for actions on gaze exit

    private bool isGazed = false;       // Whether the object is currently being gazed at
    private bool hasGazedBefore = false; // Ensures GazeEnter only triggers once

    private void Update()
    {
        // Create a forward direction based on the ray source's transform
        Vector3 direction = raySource.transform.forward;
        RaycastHit hit;

        Debug.DrawRay(raySource.transform.position, direction * rayLength, Color.red);

        // Cast a ray from the ray source
        if (Physics.Raycast(raySource.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isGazed)
                {
                    Debug.Log("Gaze Enter: " + gameObject.name);
                    isGazed = true;

                    // Only invoke the event the first time the object is gazed at
                    if (!hasGazedBefore)
                    {
                        GazeEnter.Invoke();
                        hasGazedBefore = true; // Prevent re-triggering
                    }
                }
            }
            else
            {
                ResetGaze();
            }
        }
        else
        {
            ResetGaze();
        }
    }

    private void ResetGaze()
    {
        if (isGazed)
        {
            Debug.Log("Gaze Exit: " + gameObject.name);
            isGazed = false;
            GazeExit.Invoke(); // This still happens normally
        }
    }
}
