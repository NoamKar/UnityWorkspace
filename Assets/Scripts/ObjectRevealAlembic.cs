using UnityEngine;

public class ObjectRevealAlembic : MonoBehaviour
{
    public float revealDuration = 5f; // Duration of the reveal

    private Renderer[] renderers;  // All renderers in children
    private float elapsedTime = 0f;
    private bool isRevealing = false;

    void OnEnable()
    {
        renderers = GetComponentsInChildren<Renderer>(true);

        if (renderers.Length == 0)
        {
            Debug.LogError("No renderers found on Alembic object or its children.");
            return;
        }

        SetMaterialTransparentMode();  // Force transparent mode at the start
        SetAlpha(0f);  // Start fully transparent

        isRevealing = true;
        elapsedTime = 0f;
    }

    void Update()
    {
        if (isRevealing)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / revealDuration);

            SetAlpha(alpha);

            if (elapsedTime >= revealDuration)
            {
                isRevealing = false;
                SetAlpha(1f);  // Ensure fully opaque at the end
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        foreach (Renderer renderer in renderers)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);

            if (renderer.sharedMaterial.HasProperty("_BaseColor") || renderer.sharedMaterial.HasProperty("_Color"))
            {
                Color color = renderer.sharedMaterial.HasProperty("_BaseColor") ? renderer.sharedMaterial.GetColor("_BaseColor") : renderer.sharedMaterial.GetColor("_Color");
                color.a = alpha;
                propertyBlock.SetColor(renderer.sharedMaterial.HasProperty("_BaseColor") ? "_BaseColor" : "_Color", color);
                renderer.SetPropertyBlock(propertyBlock);
            }
        }
    }

    private void SetMaterialTransparentMode()
    {
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty("_BaseColor") || mat.HasProperty("_Color"))
                {
                    mat.SetFloat("_Mode", 2);  // Set to Transparent
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = 3000;  // Transparent render queue
                }
            }
        }
    }
}
