using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;           // Speed at which the camera moves forward
    public float turnSpeed = 1f;           // Speed of the turn
    public float turnRadius = 10f;         // The radius of the turning curve
    public float finalYRotation = 90f;     // The final Y rotation angle after the turn
    public float turnDuration = 2f;        // Duration of the turn (in seconds)
    public AnimationCurve turnEase;        // Curve to ease the turn for smoothness

    private bool isTurning = false;        // Is the camera currently turning?
    private Vector3 turnCenter;            // The center point of the turning arc
    private Quaternion startRotation;      // Starting rotation before the turn
    private float elapsedTime = 0f;        // Timer to track the turn progress
    private Vector3 startPosition;         // Starting position of the turn
    private Quaternion finalRotation;      // Final rotation after the turn

    void Start()
    {
        // Ensure the easing curve is assigned
        if (turnEase == null)
        {
            turnEase = AnimationCurve.EaseInOut(0, 0, 1, 1); // Default ease-in-out curve
        }
    }

    void Update()
    {
        if (!isTurning)
        {
            // Continue moving forward if not turning
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            // Calculate how much time has passed since the turn started
            elapsedTime += Time.deltaTime;
            float turnFraction = Mathf.Clamp01(elapsedTime / turnDuration);

            // Smoothly interpolate using the easing curve
            float easedTurnFraction = turnEase.Evaluate(turnFraction);

            // Calculate the position along the curve (arc) during the turn
            Vector3 currentPosition = Vector3.Lerp(startPosition, turnCenter, easedTurnFraction);
            transform.position = currentPosition;

            // Interpolate the rotation for a smooth turn
            transform.rotation = Quaternion.Slerp(startRotation, finalRotation, easedTurnFraction);

            // Stop turning once the full turn is complete
            if (turnFraction >= 1f)
            {
                isTurning = false;
                Debug.Log("Turn complete.");
            }
        }
    }

    // Method to trigger a smooth right turn
    public void TriggerTurnRight()
    {
        if (!isTurning)
        {
            // Store initial position and rotation
            startPosition = transform.position;
            startRotation = transform.rotation;

            // Calculate the center of the turning arc based on turn radius
            turnCenter = startPosition + transform.right * turnRadius;

            // Set the final rotation (Y-axis turn)
            finalRotation = Quaternion.Euler(0f, finalYRotation, 0f);

            // Start the turning process
            isTurning = true;
            elapsedTime = 0f;  // Reset elapsed time for smooth turn
            Debug.Log("Turn triggered.");
        }
    }
}
