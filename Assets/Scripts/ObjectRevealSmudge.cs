using UnityEngine;

public class ObjectRevealSmudge : MonoBehaviour
{
    public float revealDuration = 5f; // Duration in seconds to reveal the object

    private Material[] objectMaterials;
    private float elapsedTime = 0f;
    private bool isRevealing = false;
    private Renderer[] renderers;

    void Awake()
    {
        // Get all renderers and materials in the object and its children recursively
        renderers = GetComponentsInChildren<Renderer>();
        objectMaterials = GetMaterials(renderers);

        // Set initial transparency to 0 and disable MeshRenderers
        SetTransparency(objectMaterials, 0f);
        SetRenderersEnabled(false); // Hide all renderers initially
    }

    void OnEnable()
    {
        // Reset the reveal state and timer when the object is enabled
        SetTransparency(objectMaterials, 0f);
        SetRenderersEnabled(false); // Ensure renderers are hidden initially
        isRevealing = true;
        elapsedTime = 0f;
    }

    void Update()
    {
        if (isRevealing)
        {
            if (elapsedTime < revealDuration)
            {
                // Enable renderers when fade-in starts
                if (elapsedTime == 0f)
                {
                    SetRenderersEnabled(true);
                }

                // Calculate the current transparency based on elapsed time
                float transparency = Mathf.Lerp(0f, 1f, elapsedTime / revealDuration);
                SetTransparency(objectMaterials, transparency);

                // Increment the elapsed time
                elapsedTime += Time.deltaTime;
            }
            else
            {
                // Ensure the object is fully revealed at the end of the duration
                SetTransparency(objectMaterials, 1f);
                isRevealing = false; // Stop the reveal process
            }
        }
    }

    // Retrieves materials from all renderers
    private Material[] GetMaterials(Renderer[] renderers)
    {
        Material[] materials = new Material[0];
        foreach (Renderer renderer in renderers)
        {
            Material[] rendererMaterials = renderer.materials;
            System.Array.Resize(ref materials, materials.Length + rendererMaterials.Length);
            rendererMaterials.CopyTo(materials, materials.Length - rendererMaterials.Length);
        }
        return materials;
    }

    // Sets the transparency of the materials, assuming they have a _Transparency property
    private void SetTransparency(Material[] materials, float transparency)
    {
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_Transparency"))
            {
                mat.SetFloat("_Transparency", transparency);
            }
        }
    }

    // Enable or disable all renderers in the object
    private void SetRenderersEnabled(bool enabled)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = enabled;
        }
    }
}
