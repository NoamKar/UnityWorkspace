using UnityEngine;

public class EnableObjectOnKey : MonoBehaviour
{
    public GameObject targetObject; // Assign in Inspector
    public KeyCode activationKey = KeyCode.E; // Set key in Inspector or change here

    private void Update()
    {
        if (Input.GetKeyDown(activationKey)) // Detect key press
        {
            EnableTarget();
        }
    }

    public void EnableTarget()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Target object is not assigned!");
        }
    }
}
