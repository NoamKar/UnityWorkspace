using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 1f;  // Speed of the scroll
    public float endPositionY = 10f; // The Y position where credits stop
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition; // Save the starting position
    }

    void Update()
    {
        if (transform.localPosition.y < endPositionY)
        {
            // Move the text upwards smoothly
            transform.localPosition += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
        }
        else
        {
            // Optional: Reset or disable when done
            Debug.Log("Credits finished.");
        }
    }
}
