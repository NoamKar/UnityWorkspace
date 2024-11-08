using UnityEngine;

public class ObjectHideAndDisable : MonoBehaviour
{
    public float fadeDuration = 5f; // Time to fade out the object

    private Material[] objectMaterials;
    private float elapsedTime = 0f;
    private bool isFading = false;

    void OnEnable()
    {
        // Get all materials of the object and its children
        objectMaterials = GetAllMaterials(gameObject);

        if (objectMaterials.Length == 0)
        {
            Debug.LogWarning("No materials found on the object or its children.");
            return;
        }

        // Set the initial transparency to fully visible (alpha = 1)
        SetAlpha(objectMaterials, 1f);

        // Start fading
        isFading = true;
        elapsedTime = 0f; // Reset elapsed time
        Debug.Log("Starting fade process.");
    }

    void Update()
    {
        if (isFading)
        {
            if (elapsedTime < fadeDuration)
            {
                // Calculate current alpha value
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

                // Set the alpha value to fade the object
                SetAlpha(objectMaterials, alpha);

                // Increment elapsed time
                elapsedTime += Time.deltaTime;
            }
            else
            {
                // Make sure the object is fully hidden
                SetAlpha(objectMaterials, 0f);

                // Disable the object after the fade
                gameObject.SetActive(false);

                // Stop the fade process
                isFading = false;
                Debug.Log("Fade complete, object disabled.");
            }
        }
    }

    // Get all materials of the object and its children, handling multiple materials per renderer
    private Material[] GetAllMaterials(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        int materialCount = 0;
        foreach (Renderer renderer in renderers)
        {
            materialCount += renderer.materials.Length;
        }

        Material[] materials = new Material[materialCount];
        int index = 0;
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                materials[index++] = mat;
            }
        }

        Debug.Log("Materials found: " + materials.Length);
        return materials;
    }

    // Set alpha value for each material
    private void SetAlpha(Material[] materials, float alpha)
    {
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_Color"))
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;

                // Ensure material is set to transparent mode
                mat.SetFloat("_Mode", 2); // Transparent mode
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
            else
            {
                Debug.LogWarning("Material does not have _Color property, skipping transparency.");
            }
        }
    }
}
