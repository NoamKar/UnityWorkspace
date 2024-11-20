using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [Header("Objects to Fade")]
    public GameObject model1; // First model to fade
    public GameObject model2; // Second model to fade

    [Header("Fade Settings")]
    public float fadeDuration = 2f; // Duration for fading in/out

    private Material model1Material;
    private Material model2Material;

    private void Start()
    {
        // Get the materials from the models
        model1Material = GetMaterialFromObject(model1);
        model2Material = GetMaterialFromObject(model2);

        if (model1Material == null || model2Material == null)
        {
            Debug.LogError("FadeController: One or more objects are missing a material with a '_Color' property.");
        }
    }

    // Function 1: Model 1 fades out, Model 2 fades in
    public void FadeModel1OutModel2In()
    {
        if (model1Material != null && model2Material != null)
        {
            StartCoroutine(FadeObject(model1Material, 1f, 0f));
            StartCoroutine(FadeObject(model2Material, 0f, 1f));
        }
    }

    // Function 2: Model 2 fades out, Model 1 fades in
    public void FadeModel2OutModel1In()
    {
        if (model1Material != null && model2Material != null)
        {
            StartCoroutine(FadeObject(model2Material, 1f, 0f));
            StartCoroutine(FadeObject(model1Material, 0f, 1f));
        }
    }

    private IEnumerator FadeObject(Material material, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = material.color;

        // Set the initial alpha
        color.a = startAlpha;
        material.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            color.a = alpha;
            material.color = color;
            yield return null;
        }

        // Ensure the final alpha is set
        color.a = endAlpha;
        material.color = color;

        // Do not deactivate the object to allow repeated fading
    }

    private Material GetMaterialFromObject(GameObject obj)
    {
        if (obj != null && obj.TryGetComponent<Renderer>(out Renderer renderer))
        {
            return renderer.material;
        }
        return null;
    }
}
