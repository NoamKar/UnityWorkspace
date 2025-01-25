using UnityEngine;

public class ChangeYPosition : MonoBehaviour
{
    [Header("Position Settings")]
    public float endY = 5f; // Target Y position
    public float duration = 2f; // Duration of the movement in seconds

    [Header("Movement Smoothing")]
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); // Smooth curve for easing

    private float startY; // Starting Y position (captured dynamically)
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

                transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
            }
            else
            {
                // Ensure the object ends exactly at the target position
                transform.position = new Vector3(initialPosition.x, endY, initialPosition.z);
                isMoving = false; // Stop moving
            }
        }
    }

    // Public method to start the movement
    public void StartMoving()
    {
        elapsedTime = 0f; // Reset the timer
        startY = transform.position.y; // Dynamically set startY to the current Y position
        isMoving = true;
    }
}
