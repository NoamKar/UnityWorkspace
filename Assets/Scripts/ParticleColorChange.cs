using UnityEngine;

public class ParticleColorChange : MonoBehaviour
{
    public ParticleSystem particleSystem;  // Assign the Particle System
    public Color[] colors;                 // Array of colors to fade through
    public float[] times;                  // Array of times (in seconds) when each color change should start
    public float fadeDuration = 2f;         // Duration of the fade
    public Color defaultColor = Color.white;  // Default color to reset to after play mode

    private Material targetMaterial;
    private int currentColorIndex = 0;
    private float elapsedTime = 0f;
    private Color startColor;
    private Color targetColor;
    private bool isFading = false;

    void Start()
    {
        if (particleSystem == null)
        {
            Debug.LogError("Particle System is not assigned.");
            return;
        }

        // Get the runtime material of the particle system
        ParticleSystemRenderer psRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        targetMaterial = psRenderer.material;  // Access the instance material

        if (targetMaterial == null)
        {
            Debug.LogError("Target Material could not be found on Particle System.");
            return;
        }

        if (colors.Length != times.Length)
        {
            Debug.LogError("Colors and times arrays must be the same length.");
            return;
        }

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
            startColor = targetMaterial.GetColor("_BaseColor");
            targetColor = colors[currentColorIndex];
            isFading = true;
            currentColorIndex++;
        }

        if (isFading)
        {
            float t = (elapsedTime - times[currentColorIndex - 1]) / fadeDuration;
            Color currentColor = Color.Lerp(startColor, targetColor, t);

            // Apply the interpolated color to the material instance
            targetMaterial.SetColor("_BaseColor", currentColor);

            if (t >= 1f)
            {
                isFading = false;
            }
        }
    }

    private void OnDisable()
    {
        if (targetMaterial != null && targetMaterial.HasProperty("_BaseColor"))
        {
            targetMaterial.SetColor("_BaseColor", defaultColor);
        }
    }
}
