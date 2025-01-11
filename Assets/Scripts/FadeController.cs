using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [Header("Objects to Fade")]
    public GameObject model1;  // First object to fade
    public GameObject model2;  // Second object to fade

    [Header("Fade Settings")]
    public float fadeDuration = 2f;  // Duration for fade-in/out
    public string colorPropertyName = "_BaseColor";  // Shader property name for transparency

    private Renderer[] model1Renderers;  // Array of renderers for model 1
    private Renderer[] model2Renderers;  // Array of renderers for model 2

    private void Start()
    {
        model1Renderers = GetRenderersFromObject(model1);
        model2Renderers = GetRenderersFromObject(model2);

        if (model1Renderers.Length == 0 || model2Renderers.Length == 0)
        {
            Debug.LogError("FadeController: One or more objects are missing Renderer components.");
        }
    }

    // Function 1: Fade out model 1, fade in model 2
    public void FadeModel1OutModel2In()
    {
        if (model1Renderers.Length > 0 && model2Renderers.Length > 0)
        {
            StartCoroutine(FadeOutAndDisableMeshRenderers(model1Renderers));
            StartCoroutine(EnableMeshRenderersAndFadeIn(model2Renderers));
        }
    }

    // Function 2: Fade out model 2, fade in model 1
    public void FadeModel2OutModel1In()
    {
        if (model1Renderers.Length > 0 && model2Renderers.Length > 0)
        {
            StartCoroutine(FadeOutAndDisableMeshRenderers(model2Renderers));
            StartCoroutine(EnableMeshRenderersAndFadeIn(model1Renderers));
        }
    }

    private IEnumerator EnableMeshRenderersAndFadeIn(Renderer[] renderers)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;  // Enable the MeshRenderer
        }

        yield return FadeObject(renderers, 0f, 1f);  // Fade in
    }

    private IEnumerator FadeOutAndDisableMeshRenderers(Renderer[] renderers)
    {
        yield return FadeObject(renderers, 1f, 0f);  // Fade out

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;  // Disable the MeshRenderer after fade-out
        }
    }

    private IEnumerator FadeObject(Renderer[] renderers, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            foreach (Renderer renderer in renderers)
            {
                MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor(colorPropertyName, new Color(1f, 1f, 1f, alpha));
                renderer.SetPropertyBlock(propertyBlock);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (Renderer renderer in renderers)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor(colorPropertyName, new Color(1f, 1f, 1f, endAlpha));
            renderer.SetPropertyBlock(propertyBlock);
        }
    }

    private Renderer[] GetRenderersFromObject(GameObject obj)
    {
        if (obj != null)
        {
            return obj.GetComponentsInChildren<Renderer>();  // Get all Renderer components from object and its children
        }
        return new Renderer[0];  // Return empty array if no renderers found
    }
}
