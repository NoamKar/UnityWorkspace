using UnityEngine;

public class ChangeTransformOnEnable : MonoBehaviour
{
    [Header("New Transform Values")]
    public Vector3 newPosition;  // New position for the GameObject
    public Vector3 newRotation;  // New rotation (in degrees) for the GameObject
    public Vector3 newScale = Vector3.one;  // New scale for the GameObject

    private void OnEnable()
    {
        // Change position
        transform.position = newPosition;

        // Change rotation
        transform.rotation = Quaternion.Euler(newRotation);

        // Change scale
        transform.localScale = newScale;

        Debug.Log($"{gameObject.name} transform changed on enable.");
    }
}
