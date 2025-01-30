using UnityEngine;

public class SubtitleFollowGaze : MonoBehaviour
{
    public Transform playerHead;   // Assign the VR Camera (Main Camera)
    public float distance = 2.0f;  // Distance from player
    public float followSpeed = 5.0f;  // Higher values make the movement smoother
    public Vector3 positionOffset = new Vector3(0, -0.3f, 0); // Adjust height (lower)

    private void LateUpdate()
    {
        if (playerHead == null)
        {
            Debug.LogWarning("Player head (VR camera) is not assigned.");
            return;
        }

        // Target position in front of the player's gaze
        Vector3 targetPosition = playerHead.position + playerHead.forward * distance + positionOffset;

        // Smooth movement to reduce jitter
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Make subtitle face the player
        transform.LookAt(playerHead);
        transform.Rotate(0, 180, 0); // Prevent mirrored text
    }
}
