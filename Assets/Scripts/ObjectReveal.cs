using UnityEngine;

public class ObjectReveal : MonoBehaviour
{
    public float revealDuration = 5f; // The duration in seconds to reveal the object

    private Material[] objectMaterials;
    private float elapsedTime = 0f;
    private bool isRevealing = false;

    void OnEnable()
    {
        // Get all the materials of the object
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
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / revealDuration);

            // Set the alpha value to gradually reveal the object
            SetAlpha(objectMaterials, alpha);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;
        }
        else if (isRevealing)
        {
            // Ensure the object is fully revealed at the end of the duration
            SetAlpha(objectMaterials, 1f);
            isRevealing = false; // Stop the reveal process
        }
    }

    private Material[] GetMaterials(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        Material[] materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
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
