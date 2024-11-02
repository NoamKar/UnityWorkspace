using UnityEngine;

public class MeshRendererController : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private bool areRenderersEnabled = false;  // Track renderer state

    private void Awake()
    {
        // Find all MeshRenderers in this object and all its children
        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        // Disable each MeshRenderer on Awake
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = false;
        }

        Debug.Log("All MeshRenderers disabled on Awake.");
    }

    // Public function to toggle all MeshRenderers
    public void ToggleMeshRenderers()
    {
        areRenderersEnabled = !areRenderersEnabled;

        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = areRenderersEnabled;
        }

        Debug.Log(areRenderersEnabled ? "All MeshRenderers enabled." : "All MeshRenderers disabled.");
    }
}
