using UnityEngine;

public class FadeOutCharacter : MonoBehaviour
{
    public float fadeDuration = 3f; // Duration for the fade-out effect
    private Material[] characterMaterials;
    private float elapsedTime = 0f;
    private bool isFading = true;

    void Start()
    {
        // Get the materials from the Renderer component, including children if necessary
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            characterMaterials = new Material[renderers.Length];
            int index = 0;
            foreach (Renderer renderer in renderers)
            {
                characterMaterials[index++] = renderer.material;
            }

            // Set initial transparency to 1 (fully opaque)
            SetMaterialAlpha(1f);
        }
        else
        {
            Debug.LogError("Renderer not found on the character or its children. Ensure this script is attached to an object with a Renderer component or child Renderers.");
            isFading = false;
        }
    }

    void Update()
    {
        if (isFading)
        {
            // Update elapsed time
            elapsedTime += Time.deltaTime;
            // Calculate the current alpha based on elapsed time, decreasing over time
            float alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            // Set material transparency to gradually fade out the character
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
        foreach (Material material in characterMaterials)
        {
            if (material.HasProperty("_Color"))
            {
                Color color = material.color;
                color.a = alpha; // Set the alpha value of the color
                material.color = color;
            }
        }
    }
}
