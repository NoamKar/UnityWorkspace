using UnityEngine;

public class FadeInCharacter : MonoBehaviour
{
    public float fadeDuration = 3f; // Duration for the fade-in effect
    private Material characterMaterial;
    private float elapsedTime = 0f;
    private bool isFading = true;

    void Start()
    {
        // Get the material from the Renderer component
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            characterMaterial = renderer.material;
            // Set initial transparency to 0 (fully transparent)
            SetMaterialAlpha(0f);
        }
        else
        {
            Debug.LogError("Renderer not found on the character. Attach this script to a GameObject with a Renderer component.");
            isFading = false;
        }
    }

    void Update()
    {
        if (isFading)
        {
            // Update elapsed time
            elapsedTime += Time.deltaTime;
            // Calculate the current alpha based on elapsed time
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            // Set material transparency to gradually fade in the character
            SetMaterialAlpha(alpha);

            // Stop fading when the duration is complete
            if (elapsedTime >= fadeDuration)
            {
                isFading = false;
            }
        }
    }

    private void SetMaterialAlpha(float alpha)
    {
        if (characterMaterial.HasProperty("_Color"))
        {
            Color color = characterMaterial.color;
            color.a = alpha; // Set the alpha value of the color
            characterMaterial.color = color;
        }
    }
}
