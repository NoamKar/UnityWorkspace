using System.Collections;
using UnityEngine;

public class BedMoveWithPlayer : MonoBehaviour
{
    public Transform player;           // Reference to the player's transform (or camera transform)
    public Vector3 positionOffset;     // Default offset from the player
    public Vector3 targetOffset;       // New offset to move towards
    public float moveSpeed = 2f;       // Speed of transition towards the target offset
    public Vector3 rotationOffset;     // Offset to adjust the house's rotation
    public bool followRotation = true; // Option to enable/disable rotation follow

    private bool isMovingToTarget = false; // Flag to check if moving towards target

    void LateUpdate()
    {
        // Calculate base position relative to the player
        Vector3 targetPosition = player.position + positionOffset;

        // If moving, interpolate towards the targetOffset
        if (isMovingToTarget)
        {
            positionOffset = Vector3.Lerp(positionOffset, targetOffset, moveSpeed * Time.deltaTime);

            // Stop moving when close enough
            if (Vector3.Distance(positionOffset, targetOffset) < 0.01f)
            {
                positionOffset = targetOffset;
                isMovingToTarget = false; // Stop moving
            }
        }

        // Apply the computed position
        transform.position = targetPosition;

        // Adjust rotation if enabled
        if (followRotation)
        {
            transform.rotation = player.rotation * Quaternion.Euler(rotationOffset);
        }
    }

    // Call this function to start moving towards a new offset
    public void MoveToNewOffset(Vector3 newOffset)
    {
        targetOffset = newOffset;
        isMovingToTarget = true;
    }
    public Vector3 predefinedOffset = new Vector3(0, -2f, 3f); // Adjust as needed

    // This function can be called from Timeline
    public void MoveToPredefinedOffset()
    {
        MoveToNewOffset(predefinedOffset);
    }


}
