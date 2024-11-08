using System.Collections;
using UnityEngine;

public class SmudgeEffectController : MonoBehaviour
{
    private Renderer[] renderers; // To hold all renderers in the hierarchy
    private Material[] smudgeMaterials; // Array to store all material instances

    public float smudgeDuration = 10f;   // Duration of the smudge effect
    public float finalFadeDuration = 2f; // Duration for the final fade out (0.8 to 0 transparency)
    public Vector2 smudgeDirection = new Vector2(0, -1); // Default smudge direction (downwards)

    private float smudgeAmount = 0f;    // Current smudge progress
    private float transparency = 1f;    // Current transparency
    private float smudgeOffset = 0f;    // Current smudge offset
    private bool isSmudging = false;    // Track if the smudge effect is running
    private bool smudgeComplete = false;// Track if the smudge effect is completed
    private float elapsedTime = 0f;     // Timer to track the smudge progress
    private bool isFading = false;      // Track if the final fading process has started

    void Start()
    {
        // Get all renderers in the object and its children
        renderers = GetComponentsInChildren<Renderer>();

        // Initialize the smudgeMaterials array to hold all materials from all renderers
        smudgeMaterials = new Material[GetTotalMaterialCount()];

        int index = 0;
        foreach (Renderer renderer in renderers)
        {
            // Get all materials in the current renderer and create instances
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                smudgeMaterials[index++] = materials[i];
            }
        }

        // Set initial values for transparency and smudge amount for all materials
        ResetMaterialProperties();
    }

    void Update()
    {
        if (isSmudging && !smudgeComplete)
        {
            // Update the elapsed time for smudging
            elapsedTime += Time.deltaTime;

            // Calculate smudge and transparency during smudge process
            smudgeAmount = Mathf.Lerp(0f, 1f, elapsedTime / smudgeDuration);
            transparency = Mathf.Lerp(1f, 0.8f, elapsedTime / smudgeDuration);
            smudgeOffset = Mathf.Lerp(0f, 1f, elapsedTime / smudgeDuration); // Adjust smudge offset

            // Apply the smudge amount, smudge offset, and transparency for all materials
            ApplySmudgeProperties();

            // Once smudge is complete, start final fading
            if (smudgeAmount >= 1f && !isFading)
            {
                isFading = true;
                elapsedTime = 0f; // Reset the timer for the final fade
                Debug.Log("Smudge complete, starting final fade.");
            }
        }

        // Final fade after smudge reaches 1
        if (isFading)
        {
            elapsedTime += Time.deltaTime;

            // Ensure transparency stays at 0.8 initially, then gradually fades out to 0
            transparency = Mathf.Lerp(0.8f, 0f, elapsedTime / finalFadeDuration);

            foreach (Material smudgeMaterial in smudgeMaterials)
            {
                smudgeMaterial.SetFloat("_Transparency", transparency);
            }

            // Check if final fade is complete
            if (elapsedTime >= finalFadeDuration)
            {
                smudgeComplete = true;

                foreach (Material smudgeMaterial in smudgeMaterials)
                {
                    smudgeMaterial.SetFloat("_Transparency", 0f); // Ensure transparency is fully 0
                }

                // Reset the smudge and transparency values after the object is disabled
                ResetMaterialProperties();

                // Disable the object only after the final fade completes
                gameObject.SetActive(false);
                Debug.Log("Final fade complete. Object hidden.");
            }
        }
    }

    // Public method to trigger the smudge effect
    public void TriggerSmudge()
    {
        if (!isSmudging)
        {
            Debug.Log("Smudge effect started.");
            isSmudging = true;
            elapsedTime = 0f; // Reset timer
            smudgeAmount = 0f; // Reset smudge amount
            transparency = 1f; // Reset transparency to fully visible
            smudgeOffset = 0f; // Reset smudge offset
            isFading = false; // Reset final fading state

            // Apply the smudge direction
            ApplySmudgeDirection();
        }
    }

    // Reset material properties to default
    private void ResetMaterialProperties()
    {
        smudgeAmount = 0f;
        transparency = 1f;
        smudgeOffset = 0f;

        foreach (Material smudgeMaterial in smudgeMaterials)
        {
            smudgeMaterial.SetFloat("_SmudgeAmount", smudgeAmount);
            smudgeMaterial.SetFloat("_Transparency", transparency);
            smudgeMaterial.SetFloat("_SmudgeOffset", smudgeOffset);
        }
    }

    // Apply the current smudge and transparency values to the materials
    private void ApplySmudgeProperties()
    {
        foreach (Material smudgeMaterial in smudgeMaterials)
        {
            smudgeMaterial.SetFloat("_SmudgeAmount", smudgeAmount);
            smudgeMaterial.SetFloat("_Transparency", transparency);
            smudgeMaterial.SetFloat("_SmudgeOffset", smudgeOffset);
        }
    }

    // Apply the smudge direction to all materials
    private void ApplySmudgeDirection()
    {
        foreach (Material smudgeMaterial in smudgeMaterials)
        {
            smudgeMaterial.SetVector("_SmudgeDirection", smudgeDirection);
        }
    }

    // Helper method to count total materials in all renderers
    private int GetTotalMaterialCount()
    {
        int count = 0;
        foreach (Renderer renderer in renderers)
        {
            count += renderer.materials.Length;
        }
        return count;
    }
}
