using UnityEngine;

public class MenuPositioner : MonoBehaviour
{
    public Vector3 menuOffset = new Vector3(0, 0, 2f); // 2m in front of the camera

    private void OnEnable()
    {
        PositionMenu();
    }

    private void PositionMenu()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * menuOffset.z;
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        }
        else
        {
            Debug.LogError("Main Camera not found! Ensure the camera has the 'MainCamera' tag.");
        }
    }
}
