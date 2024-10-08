using UnityEngine;

public class ObjectHideAndDisableSmudge : MonoBehaviour
{
    public float fadeDuration = 5f; // Time to fade out the object

    private Material[][] objectMaterials;
    private float elapsedTime = 0f;
    private bool isFading = false;

    void OnEnable()
    {
        // Get all materials of the object and its children
        objectMaterials = GetAllMaterials(gameObject);

        // Set the initial transparency to fully visible (_Transparency = 1)
        SetSmudgeTransparency(objectMaterials, 1f);

        // Start fading
        isFading = true;
        elapsedTime = 0f; // Reset elapsed time
    }

    void Update()
    {
        if (isFading)
        {
            if (elapsedTime < fadeDuration)
            {
                // Calculate current transparency value
                float transparency = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

                // Set the transparency value to fade the object
                SetSmudgeTransparency(objectMaterials, transparency);

                // Increment elapsed time
                elapsedTime += Time.deltaTime;
            }
            else
            {
                // Make sure the object is fully hidden
                SetSmudgeTransparency(objectMaterials, 0f);

                // Disable the object after the fade
                gameObject.SetActive(false);

                // Stop the fade process
                isFading = false;
            }
        }
    }

    // Get all materials of the object and its children, including multiple materials per renderer
    private Material[][] GetAllMaterials(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        Material[][] materials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].materials; // Get all materials of the renderer, not just one
        }
        return materials;
    }

    // Set the transparency for all materials in each renderer
    private void SetSmudgeTransparency(Material[][] materialsArray, float transparency)
    {
        foreach (Material[] materials in materialsArray)
        {
            foreach (Material mat in materials)
            {
                if (mat.HasProperty("_Transparency"))
                {
                    // Adjust the _Transparency property in the Smudge shader
                    mat.SetFloat("_Transparency", transparency);
                }
            }
        }
    }
}
