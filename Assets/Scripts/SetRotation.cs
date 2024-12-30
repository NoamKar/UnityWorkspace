using UnityEngine;

public class SetRotationOnEnable : MonoBehaviour
{
    [Header("Target Rotation")]
    public Vector3 targetRotation = new Vector3(0, 90, 0); // The desired rotation in degrees

    private void OnEnable()
    {
        // Set the GameObject's rotation to the target rotation
        transform.rotation = Quaternion.Euler(targetRotation);
        Debug.Log($"Rotation set to: {targetRotation}");
    }
}
