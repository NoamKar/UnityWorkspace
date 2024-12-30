using UnityEngine;
using UnityEngine.Events;

public class FinalGazeInteraction : MonoBehaviour
{
    public GameObject raySource;        // The object emitting the ray (e.g., Main Camera)
    public int rayLength = 15;          // Length of the ray
    public LayerMask layerMaskInteract; // Assign appropriate layers to avoid false hits

    public UnityEvent GazeEnter;        // Event for actions on gaze enter
    public UnityEvent GazeExit;         // Event for actions on gaze exit

    private bool isGazed = false;       // Whether the object is currently being gazed at

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

                    // Trigger immediate gaze enter actions
                    GazeEnter.Invoke();
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

            // Trigger gaze exit actions
            GazeExit.Invoke();
        }
    }
}
