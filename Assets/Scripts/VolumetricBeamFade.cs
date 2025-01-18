using UnityEngine;

public class LightColorAlphaFade : MonoBehaviour
{
    [Header("Light Settings")]
    public Light targetLight;               // Drag your light here
    public float fadeDuration = 2f;         // Duration of the fade
    public float startAlpha = 1f;           // Starting alpha value (0 to 1)
    public float endAlpha = 0f;             // Ending alpha value (0 to 1)

    private float elapsedTime = 0f;         // Track time during fade
    private bool isFading = false;          // Whether fading is active
    private Color initialColor;             // Store the initial light color

    private void Start()
    {
        if (targetLight == null)
        {
            Debug.LogError("No Light assigned. Please assign a Light in the Inspector.");
            enabled = false;
            return;
        }

        // Initialize the light's color
        initialColor = targetLight.color;
    }

    public void StartFade()
    {
        elapsedTime = 0f;       // Reset the timer
        isFading = true;        // Start the fade
    }

    private void Update()
    {
        if (isFading)
        {
            if (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;

                // Interpolate the alpha value
                float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

                // Update the light's color with the new alpha
                Color newColor = initialColor;
                newColor.a = Mathf.Clamp01(alpha); // Clamp to ensure valid alpha
                targetLight.color = newColor;
            }
            else
            {
                // End fading
                isFading = false;
                Debug.Log("Fade completed.");
            }
        }
    }
}
