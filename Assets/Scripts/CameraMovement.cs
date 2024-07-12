using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // Speed at which the camera moves forward
    public float turnSpeed = 100f;    // Speed at which the camera turns
    public float turnDuration = 2f;   // Duration of each turn in seconds
    public float turnInterval = 5f;   // Interval between turns in seconds

    private bool isTurning = false;

    void Start()
    {
        // Start the timed turn coroutine
        //StartCoroutine(TimedTurnCoroutine());
    }

    void Update()
    {
        // Move the camera forward continuously
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Turn the camera based on user input (e.g., left/right arrow keys or A/D keys)
        if (!isTurning)
        {
            float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
            transform.Rotate(0, turn, 0);
        }
    }

    IEnumerator TimedTurnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(turnInterval);

            // Start turning left
            isTurning = true;
            float turnTime = 0f;
            while (turnTime < turnDuration)
            {
                transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
                turnTime += Time.deltaTime;
                yield return null;
            }

            isTurning = false;

            yield return new WaitForSeconds(turnInterval);

            // Start turning right
            isTurning = true;
            turnTime = 0f;
            while (turnTime < turnDuration)
            {
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
                turnTime += Time.deltaTime;
                yield return null;
            }

            isTurning = false;
        }
    }
}
