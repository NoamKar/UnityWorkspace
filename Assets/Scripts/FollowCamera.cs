using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera's transform
    public Vector3 offsetPosition;    // Offset position relative to the camera
    public Vector3 offsetRotation;    // Offset rotation relative to the camera

    void Update()
    {
        // Update the position of the sphere based on the camera's position and the offset
        transform.position = cameraTransform.position + offsetPosition;

        // Update the rotation of the sphere based on the camera's rotation and the offset
        transform.rotation = cameraTransform.rotation * Quaternion.Euler(offsetRotation);
    }
}
