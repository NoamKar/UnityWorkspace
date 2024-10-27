using UnityEngine;
using System.Collections;

public class TreeEnterAnimation : MonoBehaviour
{
    public Animator animator;               // The Animator component on the tree
    public string animationName = "EnterAnimation"; // The name of the entry animation
    public float renderDelay = 0.01f;       // Short delay to activate the renderers

    private Renderer[] renderers;

    private void Awake()
    {
        // Automatically find and cache all renderers in the object and its children
        renderers = GetComponentsInChildren<Renderer>();

        // Initially disable all renderers to hide the object
        SetRenderersActive(false);
    }

    private void OnEnable()
    {
        // Start the animation and enable renderers after a short delay
        StartCoroutine(PlayAnimationWithRendererDelay());
    }

    private IEnumerator PlayAnimationWithRendererDelay()
    {
        // Start the animation from the first frame
        if (animator != null)
        {
            animator.Play(animationName, -1, 0f);
            animator.Update(0);  // Force it to start on the first frame immediately
        }

        // Wait for a short time to ensure the animation has started
        yield return new WaitForSeconds(renderDelay);

        // Enable all renderers to show the object with the animation playing
        SetRenderersActive(true);
    }

    private void SetRenderersActive(bool active)
    {
        // Toggle visibility of all cached renderers
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = active;
        }
    }
}
