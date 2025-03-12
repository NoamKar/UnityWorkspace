using UnityEngine;

public class ExitExperience : MonoBehaviour
{
    public void QuitApplication()
    {
        Debug.Log("Exiting Experience...");

        // Quit the application
        Application.Quit();

        // Ensure it stops play mode in Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
