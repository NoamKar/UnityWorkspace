using UnityEngine;
using UnityEngine.Events;

public class ImmidiateGazeInteraction : MonoBehaviour
{
    public GameObject raySource; // The object emitting the ray (e.g., Main Camera)
    public int rayLength = 15; // Length of the ray
    public LayerMask layerMaskInteract; // Assign appropriate layers to avoid false hits

    public Animator animator; // Reference to the Animator component (optional)
    public string triggerName; // Trigger to activate in the Animator
    public float gazeTriggerTime = 1f; // Time in seconds required to trigger the delayed response

    private bool isGazed = false; // Whether the object is currently being gazed at
    private float gazeTimer = 0f; // Timer to track gaze duration

    public UnityEvent GazeEnter; // Event for immediate actions on first gaze enter
    public UnityEvent DelayedGazeResponse; // Event for the delayed response
    public UnityEvent GazeExit; // Event for additional actions on gaze exit

    private bool hasTriggeredGazeEnter = false; // Track if GazeEnter has triggered
    private bool delayedResponseTriggered = false; // Track if the delayed response has been triggered

    private void Update()
    {
        Vector3 direction = raySource.transform.forward;
        RaycastHit hit;

        Debug.DrawRay(raySource.transform.position, direction * rayLength, Color.red);

        if (Physics.Raycast(raySource.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isGazed)
                {
                    isGazed = true;
                    gazeTimer = 0f; // Reset the timer
                    delayedResponseTriggered = false; // Reset delayed response flag

                    // **Trigger GazeEnter only if it has NOT been triggered before**
                    if (!hasTriggeredGazeEnter)
                    {
                        Debug.Log("Gaze Enter (First Time): " + gameObject.name);
                        GazeEnter.Invoke();
                        hasTriggeredGazeEnter = true; // Mark as triggered
                    }
                }

                // Increment gaze timer
                gazeTimer += Time.deltaTime;

                // **Trigger delayed gaze response if the time condition is met**
                if (gazeTimer >= gazeTriggerTime && !delayedResponseTriggered)
                {
                    TriggerDelayedGazeResponse();
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

    private void TriggerDelayedGazeResponse()
    {
        if (isGazed)
        {
            Debug.Log("Gaze Duration Met. Triggering delayed response on " + gameObject.name);
            DelayedGazeResponse.Invoke();
            delayedResponseTriggered = true; // Mark as triggered
        }
    }

    private void ResetGaze()
    {
        if (isGazed)
        {
            Debug.Log("Gaze Exit: " + gameObject.name);
            isGazed = false;
            gazeTimer = 0f; // Reset the gaze timer
            delayedResponseTriggered = false; // Reset the delayed response flag

            // **Trigger GazeExit every time the player looks away**
            GazeExit.Invoke();
        }
    }
}
