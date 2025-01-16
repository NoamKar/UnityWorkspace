using UnityEngine;

public class InvertSphere2Normals : MonoBehaviour
{
    public GameObject SferaPanoramica; // Reference to the sphere object
    private bool isNormalsInverted = false; // Ensure inversion only happens once

    private void Awake()
    {
        if (SferaPanoramica == null)
        {
            Debug.LogError("SferaPanoramica is not assigned!");
            return;
        }

        MeshFilter meshFilter = SferaPanoramica.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("No MeshFilter found on SferaPanoramica!");
            return;
        }

        // Check if inversion is needed
        if (!isNormalsInverted)
        {
            InvertMeshNormals(meshFilter.mesh);
            isNormalsInverted = true; // Prevent future inversions
            Debug.Log("Mesh normals inverted.");
        }
        else
        {
            Debug.Log("Mesh normals already inverted.");
        }
    }

    private void InvertMeshNormals(Mesh mesh)
    {
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        mesh.normals = normals;

        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int temp = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = temp;
        }
        mesh.triangles = triangles;
    }
}
