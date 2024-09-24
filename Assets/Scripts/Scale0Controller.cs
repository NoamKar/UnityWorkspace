using UnityEngine;
using System.Collections;


public class Scale0Controller : MonoBehaviour
{
    public float scaleDuration = 2f; // Time it takes to scale the object
    private Vector3 originalScale;   // Stores the original scale of the object
    private Coroutine scaleCoroutine; // Reference to any running coroutine

    void Start()
    {
        // Save the original scale of the object when the script starts
        originalScale = transform.localScale;
    }

    // Function to gradually scale the object to zero
    public void ScaleToZero()
    {
        // Stop any other ongoing scaling to prevent conflicts
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        // Start scaling to zero
        scaleCoroutine = StartCoroutine(ScaleObject(Vector3.zero));
    }

    // Function to scale the object back to its original size
    public void ScaleToOriginal()
    {
        // Stop any other ongoing scaling to prevent conflicts
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        // Start scaling back to the original size
        scaleCoroutine = StartCoroutine(ScaleObject(originalScale));
    }

    // Coroutine to handle the gradual scaling
    private IEnumerator ScaleObject(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 startingScale = transform.localScale;

        while (elapsedTime < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object is set to the exact target scale at the end
        transform.localScale = targetScale;
    }
}
