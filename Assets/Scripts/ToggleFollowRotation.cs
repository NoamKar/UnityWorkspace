using UnityEngine;

public class ToggleFollowRotation : MonoBehaviour
{
    public MoveWithPlayer moveWithPlayer;  // Reference to the MoveWithPlayer script

    void OnEnable()
    {
        // Toggle the followRotation bool when this script is enabled
        moveWithPlayer.followRotation = !moveWithPlayer.followRotation;

        // Optionally, disable this script immediately after toggling
        // if you don't want it to remain active after the toggle.
        this.enabled = false;
    }
}
