using UnityEngine;

public class MenuPositioner : MonoBehaviour
{
    public Vector3 menuOffset = new Vector3(0, 0, 2f); // 2m in front of the camera offset
    private Transform cameraOffset; // Camera Offset reference

    private void OnEnable()
    {
        FindCameraOffset();
        PositionMenu();
    }

    private void LateUpdate()
    {
        if (cameraOffset == null) FindCameraOffset(); // Ensure we always have the right cameraOffset
        PositionMenu();
    }

    private void FindCameraOffset()
    {
        GameObject offsetObject = GameObject.FindGameObjectWithTag("cameraOffset");
        if (offsetObject != null)
        {
            cameraOffset = offsetObject.transform;
        }
        else
        {
            Debug.LogError("CameraOffset not found! Ensure the GameObject has the 'CameraOffset' tag.");
        }
    }

    private void PositionMenu()
    {
        if (cameraOffset != null)
        {
            transform.position = cameraOffset.position + cameraOffset.forward * menuOffset.z;
            transform.rotation = Quaternion.LookRotation(transform.position - cameraOffset.position);
        }
    }
}
