using UnityEngine;

public class ScaleAnimator : MonoBehaviour
{
    public Vector3 finalScale = new Vector3(1f, 1f, 1f); // The target scale for the object
    public float duration = 5f; // The duration in seconds to transition to the final scale

    private Vector3 initialScale; // The initial scale of the object
    private float elapsedTime = 0f; // Tracks the time passed

    void Start()
    {
        // Record the initial scale when the script starts
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            // Calculate the current scale based on the elapsed time
            Vector3 currentScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / duration);
            transform.localScale = currentScale;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;
        }
        else
        {
            // Ensure the object reaches the final scale at the end of the duration
            transform.localScale = finalScale;
        }
    }

    public void StartScaleAnimation()
    {
        elapsedTime = 0f; // Reset the elapsed time to start the animation
        initialScale = transform.localScale; // Recalculate the initial scale in case it was changed
    }
}
