using UnityEngine;

public class CameraMoveDown : MonoBehaviour
{
    public Transform targetPosition;  // The final position the camera should move to
    public float speed = 2f;          // Speed of the camera movement
    public float delayBeforeMove = 5f; // Delay before starting the movement
    public AnimationCurve easeCurve;  // Animation curve to control easing (assign an ease-in-out curve in the inspector)
    public MonoBehaviour[] scriptsToActivate;  // Array of scripts to enable when reaching close to the target
    public float activationThreshold = 0.9f; // Percentage of the journey after which the scripts get activated

    private Vector3 startPosition;
    private float journeyLength;
    private bool isMoving = false;
    private bool scriptsActivated = false; // To track if the scripts have been activated
    private float elapsedTime = 0f;

    void Start()
    {
        if (targetPosition == null)
        {
            Debug.LogError("Target position is not set.");
            return;
        }

        // Store the initial position of the camera
        startPosition = transform.position;

        // Calculate the total distance for the camera movement
        journeyLength = Vector3.Distance(startPosition, targetPosition.position);

        // Start the movement after the specified delay
        Debug.Log("Waiting for " + delayBeforeMove + " seconds before moving the camera.");
        Invoke("StartMovingCamera", delayBeforeMove);
    }

    void Update()
    {
        if (isMoving)
        {
            // Calculate the percentage of the journey that has been completed
            elapsedTime += Time.deltaTime;
            float fracJourney = elapsedTime / (journeyLength / speed);

            // Apply easing using the AnimationCurve
            float easedFracJourney = easeCurve.Evaluate(fracJourney);

            // Move the camera towards the target position with easing
            transform.position = Vector3.Lerp(startPosition, targetPosition.position, easedFracJourney);

            // Check if the scripts need to be activated
            if (!scriptsActivated && fracJourney >= activationThreshold)
            {
                Debug.Log("Activating the scripts as we reached " + (activationThreshold * 100) + "% of the journey.");
                ActivateScripts();
                scriptsActivated = true; // Ensure they are activated only once
            }

            // Check if the camera has reached the target position
            if (fracJourney >= 1f)
            {
                Debug.Log("Camera has reached the target position.");
                isMoving = false; // Stop moving
            }
        }
    }

    void StartMovingCamera()
    {
        Debug.Log("Starting camera movement.");
        isMoving = true;
        elapsedTime = 0f; // Reset elapsed time
    }

    void ActivateScripts()
    {
        foreach (MonoBehaviour script in scriptsToActivate)
        {
            if (script != null)
            {
                script.enabled = true;
                Debug.Log("Activated script: " + script.GetType().Name);
            }
        }
    }
}
