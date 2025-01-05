using UnityEngine;

public class ShadowVolumeReveal : MonoBehaviour
{
    public float revealDuration = 5f; // The duration in seconds for the fade-in
    private Material[] objectMaterials;
    private float elapsedTime = 0f;
    private bool isRevealing = false;

    void OnEnable()
    {
        // Get all materials of the object and its children
        objectMaterials = GetMaterials(gameObject);

        // Start with fully transparent (alpha = 0)
        SetTransparency(objectMaterials, 0f);

        // Start the revealing process
        isRevealing = true;
        elapsedTime = 0f; // Reset the timer
    }

    void Update()
    {
        if (isRevealing && elapsedTime < revealDuration)
        {
            // Calculate alpha based on time
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / revealDuration);

            // Apply alpha to the object materials
            SetTransparency(objectMaterials, alpha);

            // Increment time
            elapsedTime += Time.deltaTime;
        }
        else if (isRevealing)
        {
            // Ensure fully visible at the end
            SetTransparency(objectMaterials, 1f);
            isRevealing = false; // Stop the reveal process
        }
    }

    // Retrieve all materials from the object and its children
    private Material[] GetMaterials(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        Material[] materials = new Material[0];

        foreach (Renderer renderer in renderers)
        {
            Material[] rendererMaterials = renderer.materials;
            System.Array.Resize(ref materials, materials.Length + rendererMaterials.Length);
            rendererMaterials.CopyTo(materials, materials.Length - rendererMaterials.Length);
        }

        return materials;
    }

    // Set transparency (alpha) on all materials
    private void SetTransparency(Material[] materials, float alpha)
    {
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_Color"))
            {
                // Adjust the color's alpha channel
                Color color = mat.color;
                color.a = alpha; // Set transparency
                mat.color = color;
            }

            if (mat.HasProperty("_EdgeOpacity"))
            {
                mat.SetFloat("_EdgeOpacity", alpha); // Fade edges in
            }

            if (mat.HasProperty("_ShadowIntensity"))
            {
                mat.SetFloat("_ShadowIntensity", alpha); // Increase shadow intensity
            }

            // Ensure the material renders as transparent
            mat.SetFloat("_Mode", 2); // Transparent mode
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0); // Disable depth writing for transparency
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000; // Set render queue for transparency
        }
    }
}
