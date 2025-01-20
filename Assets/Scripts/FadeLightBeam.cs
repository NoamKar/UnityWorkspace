using UnityEngine;
using UnityEngine.Rendering.Universal;


public class FadeInLightBeam : MonoBehaviour
{
    [Header("Light Beam Settings")]
    public Light lightSource;              // Assign your spotlight
    public float fadeDuration = 2f;        // Duration for the fade
    public float startAlpha = 0f;          // Starting alpha
    public float endAlpha = 1f;            // Ending alpha

    private Color initialColor;            // Store the initial light color
    private float elapsedTime = 0f;        // Track time for fading
    private bool isFading = false;         // Control fade logic

    private void Start()
    {
        if (lightSource == null)
        {
            Debug.LogError("No Light Source assigned! Please assign the light source.");
            return;
        }

        // Save the initial color of the light
        initialColor = lightSource.color;

        // Set the initial alpha to startAlpha
        SetLightAlpha(startAlpha);

        // Start fading
        isFading = true;
    }

    private void Update()
    {
        if (isFading)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the interpolated alpha value
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, t);

            // Set the light alpha
            SetLightAlpha(currentAlpha);

            // Stop fading once the duration is complete
            if (elapsedTime >= fadeDuration)
            {
                isFading = false;
                Debug.Log("Fade-in complete.");
            }
        }
    }

    private void SetLightAlpha(float alpha)
    {
        // Modify the alpha of the light's color
        if (lightSource != null)
        {
            Color color = lightSource.color;
            color.a = alpha; // Set the alpha
            lightSource.color = color;
        }
        else
        {
            Debug.LogError("Light source is missing!");
        }
    }
}
