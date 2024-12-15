using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTransition : MonoBehaviour
{
    [Header("Materials")]
    public Material originalMaterial;   // The initial material (healthy flower)
    public Material rotMaterial;       // The target material (rotted flower)

    [Header("Transition Settings")]
    public float transitionDuration = 5f; // Duration of the material transition in seconds

    private Material[] blendedMaterials;  // Array of blended materials for all renderers
    private List<Renderer> renderers;     // List of renderers to process
    private bool isTransitioning = false;

    void Start()
    {
        // Collect all Skinned Mesh Renderers in the flower model
        renderers = new List<Renderer>(GetComponentsInChildren<SkinnedMeshRenderer>());

        if (renderers.Count == 0)
        {
            Debug.LogError("No Skinned Mesh Renderers found on the flower model!");
            return;
        }

        // Create a blended material for each renderer
        blendedMaterials = new Material[renderers.Count];
        for (int i = 0; i < renderers.Count; i++)
        {
            blendedMaterials[i] = new Material(originalMaterial); // Clone original material
            renderers[i].material = blendedMaterials[i];          // Apply blended material to renderer
        }
    }

    public void StartMaterialTransition()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionMaterials());
            Debug.Log("starting material transition");
        }
    }

    private IEnumerator TransitionMaterials()
    {
        isTransitioning = true;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the blend factor (0 to 1)
            float blendFactor = elapsedTime / transitionDuration;

            // Interpolate materials for all renderers
            for (int i = 0; i < blendedMaterials.Length; i++)
            {
                blendedMaterials[i].Lerp(originalMaterial, rotMaterial, blendFactor);
            }

            yield return null;
        }

        // Ensure all materials are fully set to the rot material
        for (int i = 0; i < blendedMaterials.Length; i++)
        {
            blendedMaterials[i].Lerp(originalMaterial, rotMaterial, 1f);
        }

        isTransitioning = false;
    }
}
