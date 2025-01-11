using UnityEngine;

public class RaycastDebugLogger : MonoBehaviour
{
    public GameObject raySource;          // The object emitting the ray (e.g., Main Camera)
    public int rayLength = 15;            // Length of the ray
    public LayerMask layerMaskInteract;   // Assign appropriate layers to avoid false hits

    private bool isGazed = false;         // Whether the object is currently being gazed at

    void Update()
    {
        // Create a forward direction based on the ray source's transform
        Vector3 direction = raySource.transform.forward;
        RaycastHit hit;

        Debug.DrawRay(raySource.transform.position, direction * rayLength, Color.red);

        // Cast a ray from the ray source
        if (Physics.Raycast(raySource.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            if (hit.collider.gameObject == gameObject)  // Check if the ray hit THIS object
            {
                if (!isGazed)
                {
                    Debug.Log($"Ray hit {gameObject.name}: Gaze Entered");
                    isGazed = true;
                }
            }
            else if (isGazed)  // The ray hit something else, so trigger gaze exit
            {
                Debug.Log($"Ray stopped hitting {gameObject.name}: Gaze Exited");
                isGazed = false;
            }
        }
        else if (isGazed)  // The ray didn't hit anything, so trigger gaze exit
        {
            Debug.Log($"Ray stopped hitting {gameObject.name}: Gaze Exited (No Hit)");
            isGazed = false;
        }
    }
}
