using UnityEngine;

public class ModelTransition : MonoBehaviour
{
    public GameObject completeModel; // The complete character model
    public GameObject brokenModel; // The broken character model
    public float transitionDuration = 5f; // Duration of the fade transition

    private Material[] completeModelMaterials;
    private Material[] brokenModelMaterials;
    private float elapsedTime = 0f;

    void Start()
    {
        // Get the materials of both models
        completeModelMaterials = GetMaterials(completeModel);
        brokenModelMaterials = GetMaterials(brokenModel);

        // Set the initial transparency
        SetAlpha(completeModelMaterials, 1f); // Fully opaque
        SetAlpha(brokenModelMaterials, 0f); // Fully transparent
    }

    void Update()
    {
        if (elapsedTime < transitionDuration)
        {
            // Calculate the current alpha value based on elapsed time
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionDuration);

            // Set the alpha values for both models
            SetAlpha(completeModelMaterials, alpha);
            SetAlpha(brokenModelMaterials, 1f - alpha);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;
        }
        else
        {
            // Ensure final values are set
            SetAlpha(completeModelMaterials, 0f);
            SetAlpha(brokenModelMaterials, 1f);
        }
    }

    private Material[] GetMaterials(GameObject model)
    {
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
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

    public void StartFadeTransition()
    {
        elapsedTime = 0f; // Reset the elapsed time to start the fade transition
    }
}
