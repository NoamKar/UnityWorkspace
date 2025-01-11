using UnityEngine;

public class ScaleOverTime : MonoBehaviour
{
    [Header("Scale Settings")]
    public Vector3 startScale = Vector3.one;  // Initial scale of the object
    public Vector3 endScale = Vector3.one * 2f;  // Final scale of the object
    public float scaleDuration = 2f;  // Duration in seconds to reach the final scale

    private float elapsedTime = 0f;
    private bool isScaling = false;

    private void OnEnable()
    {
        // Reset the scale to the start value
        transform.localScale = startScale;

        // Start the scaling process
        isScaling = true;
        elapsedTime = 0f;
    }

    private void Update()
    {
        if (isScaling)
        {
            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the current scale using Lerp
            float progress = Mathf.Clamp01(elapsedTime / scaleDuration);
            transform.localScale = Vector3.Lerp(startScale, endScale, progress);

            // Stop scaling when the duration is reached
            if (progress >= 1f)
            {
                isScaling = false;
            }
        }
    }

    // Public method to manually trigger scaling if needed
    public void StartScaling()
    {
        OnEnable();
    }
}
