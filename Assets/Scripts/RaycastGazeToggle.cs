using UnityEngine;
using UnityEngine.Events;

public class RaycastGazeToggle : MonoBehaviour
{
    public GameObject raySource;  // The object emitting the ray (e.g., Main Camera)
    public GameObject targetObject;  // The object to enable/disable
    public int rayLength = 15;    // Length of the ray
    public LayerMask layerMaskInteract; // Assign appropriate layers to avoid false hits
    public float gazeExitBufferTime = 0.2f;  // Time to wait before triggering GazeExit

    private bool isGazed = false;
    private bool isFirstGaze = true; // Tracks whether it's the first gaze or not
    private float exitTimer = 0f;

    public UnityEvent FirstGazeEnter;  // Event for the first gaze
    public UnityEvent SubsequentGazeEnter; // Event for subsequent gazes
    public UnityEvent FirstGazeExit;   // Event for the first gaze exit
    public UnityEvent SubsequentGazeExit;  // Event for subsequent gaze exits

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
                    if (isFirstGaze)
                    {
                        // Handle first gaze
                        Debug.Log("First Gaze Enter: " + this.gameObject.name);
                        FirstGazeEnter.Invoke();
                        isFirstGaze = false; // Ensure next gazes are considered subsequent
                    }
                    else
                    {
                        // Handle subsequent gazes
                        Debug.Log("Subsequent Gaze Enter: " + this.gameObject.name);
                        SubsequentGazeEnter.Invoke();
                    }

                    isGazed = true;
                    exitTimer = 0f;  // Reset the exit timer

                    // Activate the target object
                    if (targetObject != null)
                    {
                        targetObject.SetActive(true);
                    }
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
            Debug.Log("Gaze Exit: " + this.gameObject.name);

            if (isFirstGaze)
            {
                // First exit handling
                FirstGazeExit.Invoke();
            }
            else
            {
                // Subsequent exit handling
                SubsequentGazeExit.Invoke();
            }

            isGazed = false;

            // Disable the target object
            if (targetObject != null)
            {
                targetObject.SetActive(false);
            }
        }
    }
}
