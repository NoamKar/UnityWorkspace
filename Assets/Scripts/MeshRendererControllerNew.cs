using UnityEngine;

public class MeshRendererControllerNew : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;

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

    // Public function to enable all MeshRenderers
    public void EnableMeshRenderers()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = true;
        }

        Debug.Log("All MeshRenderers enabled.");
    }

    public void DisableMeshRenderers()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = false;
        }

        Debug.Log("All MeshRenderers disabled.");
    }

}
