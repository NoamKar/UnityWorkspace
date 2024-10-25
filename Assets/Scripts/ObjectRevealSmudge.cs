using UnityEngine;

public class ObjectRevealSmudge : MonoBehaviour
{
    public float revealDuration = 5f; // The duration in seconds to reveal the object

    private Material[] objectMaterials;
    private float elapsedTime = 0f;
    private bool isRevealing = false;

    void OnEnable()
    {
        // Get all the materials of the object and its children recursively
        objectMaterials = GetMaterials(gameObject);

        // Set the initial transparency to 0 (fully transparent)
        SetTransparency(objectMaterials, 0f);

        // Start revealing
        isRevealing = true;
        elapsedTime = 0f; // Reset the elapsed time whenever the object is enabled
    }

    void Update()
    {
        if (isRevealing && elapsedTime < revealDuration)
        {
            // Calculate the current transparency value based on elapsed time
            float transparency = Mathf.Lerp(0f, 1f, elapsedTime / revealDuration);

            // Set the transparency value to gradually reveal the object and its children
            SetTransparency(objectMaterials, transparency);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;
        }
        else if (isRevealing)
        {
            // Ensure the object is fully revealed at the end of the duration
            SetTransparency(objectMaterials, 1f);
            isRevealing = false; // Stop the reveal process
        }
    }

    // This method now retrieves materials from the object and all of its children recursively
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

    // Set the transparency value of the material, assuming it has the _Transparency property
    private void SetTransparency(Material[] materials, float transparency)
    {
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_Transparency"))
            {
                // Adjust the _Transparency property in the shader
                mat.SetFloat("_Transparency", transparency);
            }
        }
    }
}
