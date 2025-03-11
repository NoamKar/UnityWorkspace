using UnityEngine;
using UnityEngine.UI;

public class MenuAutoCamera : MonoBehaviour
{
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        UpdateEventCamera();
    }

    private void OnEnable()
    {
        UpdateEventCamera();
    }

    private void UpdateEventCamera()
    {
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                canvas.worldCamera = mainCamera;
            }
            else
            {
                Debug.LogWarning("Main Camera not found! Ensure your camera has the 'MainCamera' tag.");
            }
        }
    }
}
