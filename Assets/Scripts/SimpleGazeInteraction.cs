using UnityEngine;
using UnityEngine.Events;

public class SimpleGazeInteraction : MonoBehaviour
{
    public GameObject raySource;                // The object emitting the ray (e.g., Main Camera)
    public int rayLength = 15;                  // Length of the ray
    public LayerMask layerMaskInteract;         // Assign appropriate layers to avoid false hits

    public Animator animator;                   // Reference to the Animator component (optional)
    public string triggerName;                  // Trigger to activate in the Animator
    public float gazeTriggerTime = 1f;          // Time in seconds required to trigger the animation

    private bool isGazed = false;               // Whether the object is currently being gazed at
    private float gazeTimer = 0f;               // Timer to track gaze duration
    private bool hasTriggeredGazeEnter = false; // Ensure GazeEnter only happens once

    public UnityEvent GazeEnter;                // Event for additional actions on gaze enter (once)
    public UnityEvent GazeExit;                 // Event for additional actions on gaze exit

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
                    gazeTimer = 0f;  // Reset the timer on gaze enter

                    // Only trigger GazeEnter **once** per object
                    if (!hasTriggeredGazeEnter)
                    {
                        hasTriggeredGazeEnter = true;
                        GazeEnter.Invoke();
                    }
                }

                // Increment gaze timer
                gazeTimer += Time.deltaTime;

                // Check if gaze duration requirement is met
                if (gazeTimer >= gazeTriggerTime)
                {
                    TriggerGazeEffect();
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

    private void TriggerGazeEffect()
    {
        if (isGazed && animator != null && !string.IsNullOrEmpty(triggerName))
        {
            Debug.Log("Gaze Duration Met. Triggering animation on " + gameObject.name);
            animator.SetTrigger(triggerName);  // Trigger the animation
            ResetGaze(); // Reset after triggering to avoid re-triggering
        }
    }

    private void ResetGaze()
    {
        if (isGazed)
        {
            Debug.Log("Gaze Exit: " + gameObject.name);
            isGazed = false;
            gazeTimer = 0f; // Reset the gaze timer

            // **GazeExit will still trigger each time**
            GazeExit.Invoke();
        }
    }
}
