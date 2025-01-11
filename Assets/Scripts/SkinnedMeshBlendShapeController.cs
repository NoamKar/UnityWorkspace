using UnityEngine;

public class SkinnedMeshBlendShapeController : MonoBehaviour
{
    [Header("Skinned Mesh Renderer Settings")]
    public SkinnedMeshRenderer skinnedMeshRenderer;  // Reference to the Skinned Mesh Renderer component

    [Header("Blend Shape Settings")]
    public int blendShapeIndex1 = 0;                 // Index of the first blend shape
    public int blendShapeIndex2 = 1;                 // Index of the second blend shape
    public float blendDuration = 2f;                 // Duration for the blend shape animation

    [Range(0f, 100f)] public float blendShape1StartValue = 0f;   // Starting value for blend shape 1
    [Range(0f, 100f)] public float blendShape1EndValue = 100f;  // Ending value for blend shape 1

    [Range(0f, 100f)] public float blendShape2StartValue = 0f;   // Starting value for blend shape 2
    [Range(0f, 100f)] public float blendShape2EndValue = 100f;  // Ending value for blend shape 2

    [Header("Breathing Effect Settings")]
    public bool enableBreathing = false;             // Enable/Disable breathing effect
    public float breathingSpeed = 1f;                // Speed of the breathing effect
    public float breathingMinValue = 0f;             // Minimum random value for breathing
    public float breathingMaxValue = 100f;           // Maximum random value for breathing

    private void Start()
    {
        if (skinnedMeshRenderer == null)
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("No SkinnedMeshRenderer assigned or found on this GameObject.");
        }

        if (enableBreathing)
        {
            StartBreathingEffect();
        }
    }

    public void StartBlendShapeAnimation()
    {
        if (skinnedMeshRenderer != null)
        {
            StopBreathingEffect(); // Stop breathing if active
            StartCoroutine(AnimateBlendShapes(blendShapeIndex1, blendShape1StartValue, blendShape1EndValue,
                                              blendShapeIndex2, blendShape2StartValue, blendShape2EndValue));
        }
    }

    private System.Collections.IEnumerator AnimateBlendShapes(int index1, float startValue1, float endValue1,
                                                               int index2, float startValue2, float endValue2)
    {
        float elapsedTime = 0f;

        while (elapsedTime < blendDuration)
        {
            float blendValue1 = Mathf.Lerp(startValue1, endValue1, elapsedTime / blendDuration);
            float blendValue2 = Mathf.Lerp(startValue2, endValue2, elapsedTime / blendDuration);

            skinnedMeshRenderer.SetBlendShapeWeight(index1, blendValue1);
            skinnedMeshRenderer.SetBlendShapeWeight(index2, blendValue2);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        skinnedMeshRenderer.SetBlendShapeWeight(index1, endValue1);
        skinnedMeshRenderer.SetBlendShapeWeight(index2, endValue2);
    }

    public void StartBreathingEffect()
    {
        enableBreathing = true;
        StartCoroutine(BreathingEffect());
    }

    public void StopBreathingEffect()
    {
        enableBreathing = false;
        StopAllCoroutines(); // Stop all coroutines, including breathing
    }

    private System.Collections.IEnumerator BreathingEffect()
    {
        while (enableBreathing)
        {
            float randomTarget1 = Random.Range(breathingMinValue, breathingMaxValue);
            float randomTarget2 = Random.Range(breathingMinValue, breathingMaxValue);

            float currentWeight1 = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex1);
            float currentWeight2 = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex2);

            float elapsedTime = 0f;

            while (elapsedTime < breathingSpeed && enableBreathing)
            {
                float blendValue1 = Mathf.Lerp(currentWeight1, randomTarget1, elapsedTime / breathingSpeed);
                float blendValue2 = Mathf.Lerp(currentWeight2, randomTarget2, elapsedTime / breathingSpeed);

                skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex1, blendValue1);
                skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex2, blendValue2);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex1, randomTarget1);
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex2, randomTarget2);
        }
    }
}
