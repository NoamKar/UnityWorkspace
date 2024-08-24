using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour
{
    public Transform player;           // Reference to the player's transform (or camera transform)
    public Vector3 positionOffset;     // Offset to keep the house relative to the player's position
    public Vector3 rotationOffset;     // Offset to adjust the house's rotation relative to the player's rotation

    void LateUpdate()
    {
        // Keep the house object at the player's position with the specified position offset
        transform.position = player.position + positionOffset;

        // Adjust the rotation of the house relative to the player's rotation with an additional rotation offset
        transform.rotation = player.rotation * Quaternion.Euler(rotationOffset);
    }
}
