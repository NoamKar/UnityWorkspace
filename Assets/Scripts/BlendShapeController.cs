using UnityEngine;

public class BlendShapeController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer; // The SkinnedMeshRenderer component of your model
    public int blendShapeIndex = 0; // The index of the blend shape you want to animate
    public int startBlendValue = 0; // Starting blend shape value
    public int endBlendValue = 100; // Ending blend shape value
    public float duration = 5f; // Duration in seconds to transition from start to end

    private float elapsedTime = 0f; // Tracks the time passed

    void Update()
    {
        if (elapsedTime < duration)
        {
            // Calculate the current blend shape value based on the elapsed time
            float blendValue = Mathf.Lerp(startBlendValue, endBlendValue, elapsedTime / duration);
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, blendValue);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;
        }
        else
        {
            // Ensure the blend shape reaches the final value at the end of the duration
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, endBlendValue);
        }
    }

    public void StartBlendShapeAnimation()
    {
        elapsedTime = 0f; // Reset the elapsed time to start the animation
    }
}
