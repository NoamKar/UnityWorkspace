using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 0.4f;           // Speed at which the camera moves forward
    public float turnSpeed = 10f;          // Speed of the turn (degrees per second)
    public float turnDuration = 5f;        // Duration of the turn in seconds
    public float turnRadius = 10f;         // Radius of the turn (how wide the turn is)
    public float finalYRotation = 50f;     // The final Y rotation angle after the turn
    public bool affectXAxis = false;        // Whether to affect movement on the X-axis
    public bool affectZAxis = true;        // Whether to affect movement on the Z-axis

    private bool isTurning = false;        // Is the camera currently turning?
    private Vector3 turnCenter;            // The center point of the turning arc
    private float turnStartTime;           // Time when the turn started
    private Vector3 originalPosition;      // Store the original position before the turn
    private Quaternion originalRotation;   // Store the original rotation before the turn

    void Update()
    {
        if (isTurning)
        {
            // Calculate how much time has passed since the turn started
            float timeSinceTurnStart = Time.time - turnStartTime;

            // Calculate the fraction of the turn duration that has been completed
            float turnFraction = Mathf.Clamp01(timeSinceTurnStart / turnDuration);

            // Calculate the angle we should have turned based on the turnFraction and finalYRotation
            float angleThisFrame = Mathf.Lerp(0f, finalYRotation, turnFraction);

            // Calculate the offset for the position (circular turn)
            Vector3 offset = new Vector3(
                affectXAxis ? Mathf.Sin(Mathf.Deg2Rad * angleThisFrame) * turnRadius : 0,
                0,
                affectZAxis ? Mathf.Cos(Mathf.Deg2Rad * angleThisFrame) * turnRadius : 0
            );

            // Update the position along the chosen axes
            transform.position = turnCenter - offset;

            // Update the rotation to smoothly turn
            transform.rotation = Quaternion.Slerp(originalRotation, Quaternion.Euler(0f, finalYRotation, 0f), turnFraction);

            // Check if the turn has completed
            if (turnFraction >= 1f)
            {
                isTurning = false;
                Debug.Log("Turn complete.");
            }
        }
        else
        {
            // Continue forward movement if not turning
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    // Method to trigger the right turn
    public void TriggerTurnRight()
    {
        if (!isTurning)
        {
            // Save the original position and rotation
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            // Calculate the center of the turning arc (based on whether X or Z axis is affected)
            turnCenter = originalPosition - (affectXAxis ? transform.right : Vector3.zero) * turnRadius;

            // Set the start time of the turn
            turnStartTime = Time.time;

            // Start turning
            isTurning = true;
            Debug.Log("Turn triggered.");
        }
    }
}
