using UnityEngine;
using UnityEngine.Events;

public class RaycastEvents_VR : MonoBehaviour
{
    public GameObject raySource;  // The object emitting the ray (e.g., Main Camera)
    public int rayLength = 15;    // Length of the ray
    public LayerMask layerMaskInteract; // Assign appropriate layers to avoid false hits
    public float gazeExitBufferTime = 0.2f;  // Time to wait before triggering GazeExit

    public Animator animator; // Reference to the Animator component
    public string[] triggersToReset; // List of triggers to reset after gaze cycle
    public string conditionalTrigger; // The trigger that, when invoked, will reset the other triggers

    private bool isGazed = false;
    private float exitTimer = 0f;

    public UnityEvent GazeEnter;
    public UnityEvent GazeExit;

    private void Update()
    {
        // Create a forward direction based on the ray source's transform
        Vector3 direction = raySource.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        Debug.DrawRay(raySource.transform.position, direction * rayLength, Color.red);

        // Cast a ray from the ray source
        if (Physics.Raycast(raySource.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                if (!isGazed)
                {
                    // The first time the ray hits the object, invoke GazeEnter
                    Debug.Log("Gaze Enter: " + this.gameObject.name);
                    GazeEnter.Invoke();
                    isGazed = true;
                    exitTimer = 0f;  // Reset the exit timer
                }
                else
                {
                    // Reset the exit timer if still gazing at the object
                    exitTimer = 0f;
                }
            }
            else
            {
                // If the ray was previously hitting the object but no longer is, start the exit timer
                exitTimer += Time.deltaTime;
                if (exitTimer >= gazeExitBufferTime)
                {
                    HandleGazeExit();
                }
            }
        }
        else
        {
            // If the ray was previously hitting the object but no longer is, start the exit timer
            exitTimer += Time.deltaTime;
            if (exitTimer >= gazeExitBufferTime)
            {
                HandleGazeExit();
            }
        }
    }

    private void HandleGazeExit()
    {
        if (isGazed)
        {
            // Gaze has truly exited, invoke the GazeExit event
            Debug.Log("Gaze Exit: " + this.gameObject.name);
            GazeExit.Invoke();
            isGazed = false;
        }
    }

    // Public method to invoke the conditional trigger and reset the hover triggers
    public void InvokeConditionalTriggerAndReset()
    {
        Debug.Log("Conditional Trigger Invoked: " + conditionalTrigger);
        animator.SetTrigger(conditionalTrigger); // Invoke the conditional trigger

        ResetHoverTriggers(); // Reset the other triggers after invoking the conditional trigger
    }

    // Private method to reset the triggers after the conditional trigger is invoked
    private void ResetHoverTriggers()
    {
        Debug.Log("Resetting triggers");
        foreach (string trigger in triggersToReset)
        {
            animator.ResetTrigger(trigger);
        }
    }
}
