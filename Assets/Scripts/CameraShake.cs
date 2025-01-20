using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeDuration = 1f;        // Duration of the shake effect
    public float shakeMagnitude = 0.1f;    // Intensity of the shake
    public float dampingSpeed = 1f;        // How quickly the shake diminishes

    private Vector3 originalPosition;      // The camera's initial position
    private float currentShakeDuration = 0f; // Tracks remaining shake time
    private bool isShaking = false;        // Tracks if shake is active

    void Start()
    {
        // Store the camera's initial position
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            if (currentShakeDuration > 0)
            {
                // Generate random offset for shake effect
                Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;

                // Apply the shake effect
                transform.localPosition = originalPosition + shakeOffset;

                // Decrease the shake duration over time
                currentShakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                // Reset the position and stop shaking
                transform.localPosition = originalPosition;
                isShaking = false;
            }
        }
    }

    // Public method to trigger the shake effect
    public void TriggerShake(float duration = 0f)
    {
        currentShakeDuration = duration > 0 ? duration : shakeDuration;
        isShaking = true;
    }
}
