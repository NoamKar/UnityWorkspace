using UnityEngine;

public class BgColorChange : MonoBehaviour
{
    public Camera targetCamera;
    public Color[] colors;     // Array of colors to fade through
    public float[] times;      // Array of times (in seconds) when each color change should start
    public float fadeDuration = 2f; // Duration of the fade between colors

    private int currentColorIndex = 0;
    private float elapsedTime = 0f;
    private Color startColor;
    private Color targetColor;
    private bool isFading = false;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main; // Default to main camera if none assigned
        }

        if (colors.Length != times.Length)
        {
            Debug.LogError("Colors and times arrays must be the same length.");
            return;
        }

        // Initialize with the first color
        if (colors.Length > 0)
        {
            targetCamera.backgroundColor = colors[0];
            startColor = colors[0];
            targetColor = colors[0];
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (currentColorIndex < times.Length && elapsedTime >= times[currentColorIndex])
        {
            // Start the fade to the next color
            startColor = targetCamera.backgroundColor;
            targetColor = colors[currentColorIndex];
            isFading = true;
            currentColorIndex++;
        }

        if (isFading)
        {
            // Calculate the interpolation factor
            float t = (elapsedTime - times[currentColorIndex - 1]) / fadeDuration;
            targetCamera.backgroundColor = Color.Lerp(startColor, targetColor, t);

            // Stop fading once the duration has passed
            if (t >= 1f)
            {
                isFading = false;
            }
        }
    }
}
