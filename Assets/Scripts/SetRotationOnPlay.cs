using UnityEngine;

public class SetRotationOnPlay : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationOnPlay; // Public value to set rotation (in degrees)

    void Start()
    {
        // Apply the specified rotation when play mode starts
        transform.localRotation = Quaternion.Euler(rotationOnPlay);
    }
}
