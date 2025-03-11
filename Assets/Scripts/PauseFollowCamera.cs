using UnityEngine;

public class PauseFollowCamera : MonoBehaviour
{
    private Transform cameraTransform;  // Reference to the Main Camera's transform
    public Vector3 positionOffset = new Vector3(0, -0.2f, 0.5f);  // Offset relative to the camera
    public bool followRotation = false; // Set to false to prevent rotation following

    private void Start()
    {
        FindCamera();
    }

    private void OnEnable()
    {
        FindCamera();
    }

    private void FindCamera()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("Camera found: " + cameraTransform.name);
        }
        else
        {
            Debug.LogWarning("Main Camera not found! Ensure your camera has the 'MainCamera' tag.");
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null)
        {
            FindCamera(); // Keep checking for the camera in case it wasn't found initially
            return;
        }

        // Maintain the world-space position offset relative to the camera’s position
        transform.position = cameraTransform.position + positionOffset;

        // Do NOT rotate with the camera (keep world rotation)
    }
}
