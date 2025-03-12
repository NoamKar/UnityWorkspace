using UnityEngine;

public class MeshRendererControllerNew : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private void Awake()
    {
        // Find all MeshRenderers and SkinnedMeshRenderers in this object and all its children
        meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);

        // Disable all renderers on Awake
        DisableMeshRenderers();

        Debug.Log($"Disabled {meshRenderers.Length} MeshRenderers and {skinnedMeshRenderers.Length} SkinnedMeshRenderers on Awake.");
    }

    // Public function to enable all MeshRenderers and SkinnedMeshRenderers
    public void EnableMeshRenderers()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            if (renderer != null) renderer.enabled = true;
        }

        foreach (SkinnedMeshRenderer skinnedRenderer in skinnedMeshRenderers)
        {
            if (skinnedRenderer != null) skinnedRenderer.enabled = true;
        }

        Debug.Log("All MeshRenderers and SkinnedMeshRenderers enabled.");
    }

    public void DisableMeshRenderers()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            if (renderer != null) renderer.enabled = false;
        }

        foreach (SkinnedMeshRenderer skinnedRenderer in skinnedMeshRenderers)
        {
            if (skinnedRenderer != null) skinnedRenderer.enabled = false;
        }

        Debug.Log("All MeshRenderers and SkinnedMeshRenderers disabled.");
    }
}
