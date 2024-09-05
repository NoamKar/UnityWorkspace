using UnityEngine;

public class ParticleColorChange : MonoBehaviour
{
    public Material targetMaterial;  // The material you want to modify
    public Color[] colors;           // Array of colors to fade through
    public float[] times;            // Array of times (in seconds) when each color change should start
    public float fadeDuration = 2f;  // Duration of the fade
    public Color defaultColor = Color.white;  // Default color to reset to after play mode

    private int currentColorIndex = 0;
    private float elapsedTime = 0f;
    private Color startColor;
    private Color targetColor;
    private bool isFading = false;

    void Start()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Target Material is not assigned.");
            return;
        }

        if (colors.Length != times.Length)
        {
            Debug.LogError("Colors and times arrays must be the same length.");
            return;
        }

        // Get the initial color of the material
        if (targetMaterial.HasProperty("_BaseColor"))
        {
            startColor = targetMaterial.GetColor("_BaseColor");
        }
        else
        {
            Debug.LogError("Material does not have a _BaseColor property.");
            return;
        }

        // Initialize with the first target color
        if (colors.Length > 0)
        {
            targetColor = colors[0];
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Check if it's time to start fading to the next color
        if (currentColorIndex < times.Length && elapsedTime >= times[currentColorIndex])
        {
            // Start the fade to the next color
            startColor = targetMaterial.GetColor("_BaseColor");
            targetColor = colors[currentColorIndex];
            isFading = true;
            currentColorIndex++;
        }

        if (isFading)
        {
            // Calculate the interpolation factor
            float t = (elapsedTime - times[currentColorIndex - 1]) / fadeDuration;
            Color currentColor = Color.Lerp(startColor, targetColor, t);

            // Apply the interpolated color to the material
            targetMaterial.SetColor("_BaseColor", currentColor);

            // Stop fading once the duration has passed
            if (t >= 1f)
            {
                isFading = false;
            }
        }
    }

    // This function is called when exiting play mode or when the script is disabled
    private void OnDisable()
    {
        // Reset the material to the default color
        if (targetMaterial != null && targetMaterial.HasProperty("_BaseColor"))
        {
            targetMaterial.SetColor("_BaseColor", defaultColor);
        }
    }
}
