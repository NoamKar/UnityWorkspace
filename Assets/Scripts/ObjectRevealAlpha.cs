using UnityEngine;

public class ObjectRevealAlpha : MonoBehaviour
{
    [Header("Reveal Settings")]
    public float revealDuration = 5f; // The duration in seconds to reveal the object
    [Range(0f, 1f)] public float finalAlpha = 1f; // Final alpha value (fully transparent to fully opaque)

    private Material[] objectMaterials;
    private float elapsedTime = 0f;
    private bool isRevealing = false;

    void OnEnable()
    {
        // Get all the materials of the object and its children recursively
        objectMaterials = GetMaterials(gameObject);

        // Set the initial transparency to 0 (fully transparent)
        SetAlpha(objectMaterials, 0f);

        // Start revealing
        isRevealing = true;
        elapsedTime = 0f; // Reset the elapsed time whenever the object is enabled
    }

    void Update()
    {
        if (isRevealing && elapsedTime < revealDuration)
        {
            // Calculate the current alpha value based on elapsed time
            float alpha = Mathf.Lerp(0f, finalAlpha, elapsedTime / revealDuration);

            // Set the alpha value to gradually reveal the object and its children
            SetAlpha(objectMaterials, alpha);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;
        }
        else if (isRevealing)
        {
            // Ensure the object is revealed at the final alpha level at the end of the duration
            SetAlpha(objectMaterials, finalAlpha);
            isRevealing = false; // Stop the reveal process
        }
    }

    // This method retrieves materials from the object and all of its children recursively
    private Material[] GetMaterials(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(); // Get all renderers in the object and its children
        Material[] materials = new Material[0];

        foreach (Renderer renderer in renderers)
        {
            Material[] rendererMaterials = renderer.materials; // Get materials of each renderer
            System.Array.Resize(ref materials, materials.Length + rendererMaterials.Length);
            rendererMaterials.CopyTo(materials, materials.Length - rendererMaterials.Length);
        }

        return materials;
    }

    private void SetAlpha(Material[] materials, float alpha)
    {
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_Color"))
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;

                // Ensure the material is set to transparent mode
                mat.SetFloat("_Mode", 2); // Set to Transparent mode
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
        }
    }
}
