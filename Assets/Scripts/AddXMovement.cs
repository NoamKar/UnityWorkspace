using UnityEngine;

public class AddXMovement : MonoBehaviour
{
    public float moveDistance = 1f;        // Total distance to move on the X-axis
    public float moveDuration = 2f;       // Duration of the X-axis movement (in seconds)
    public bool loopMovement = true;      // Option to loop the movement
    public int moveDirection = 1;         // Direction of movement: 1 (right), -1 (left)

    private float elapsedTime = 0f;
    private int directionMultiplier = 1; // Used for reversing direction in looping
    private Vector3 originalOffset;

    private void Start()
    {
        // Save the original position offset to ensure consistent movement
        if (TryGetComponent<MoveWithPlayer>(out MoveWithPlayer moveWithPlayer))
        {
            originalOffset = moveWithPlayer.positionOffset;
        }
        else
        {
            Debug.LogError("AddXMovement requires the MoveWithPlayer script to work!");
            enabled = false;
        }
    }

    private void Update()
    {
        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the new X offset
        float xOffset = Mathf.Lerp(0, moveDistance * moveDirection * directionMultiplier, elapsedTime / moveDuration);

        // Reverse direction and reset the timer if looping is enabled
        if (elapsedTime >= moveDuration)
        {
            if (loopMovement)
            {
                elapsedTime = 0f;
                directionMultiplier *= -1; // Reverse direction
            }
            else
            {
                enabled = false; // Stop the movement if looping is disabled
            }
        }

        // Apply the new offset to the MoveWithPlayer's positionOffset
        if (TryGetComponent<MoveWithPlayer>(out MoveWithPlayer moveWithPlayer))
        {
            moveWithPlayer.positionOffset = originalOffset + new Vector3(xOffset, 0, 0);
        }
    }
}
