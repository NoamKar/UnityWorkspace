using UnityEngine;

public class ChangeYZPosition : MonoBehaviour
{
    [Header("Position Settings")]
    public float endY = 5f; // Target Y position
    public float endZ = 5f; // Target Z position
    public float duration = 2f; // Duration of the movement in seconds

    [Header("Movement Smoothing")]
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); // Smooth curve for easing

    private float startY; // Starting Y position
    private float startZ; // Starting Z position
    private float elapsedTime = 0f; // Track elapsed time
    private bool isMoving = false; // Flag to control movement

    private Vector3 initialPosition; // Cache the initial position

    void Start()
    {
        // Store the initial position of the object
        initialPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            if (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                // Calculate the normalized time (0 to 1)
                float normalizedTime = elapsedTime / duration;

                // Use the animation curve to adjust movement
                float curveValue = movementCurve.Evaluate(normalizedTime);
                float newY = Mathf.Lerp(startY, endY, curveValue);
                float newZ = Mathf.Lerp(startZ, endZ, curveValue);

                transform.position = new Vector3(initialPosition.x, newY, newZ);
            }
            else
            {
                // Ensure the object ends exactly at the target positions
                transform.position = new Vector3(initialPosition.x, endY, endZ);
                isMoving = false; // Stop moving
            }
        }
    }

    // Public method to start the movement
    public void StartMoving()
    {
        elapsedTime = 0f; // Reset the timer
        startY = transform.position.y; // Set startY to the current Y position
        startZ = transform.position.z; // Set startZ to the current Z position
        isMoving = true;
    }
}
