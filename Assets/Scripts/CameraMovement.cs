using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // Speed at which the camera moves forward


    void Start()
    {
        // Start the timed turn coroutine
        //StartCoroutine(TimedTurnCoroutine());
    }

    void Update()
    {
        // Move the camera forward continuously
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
