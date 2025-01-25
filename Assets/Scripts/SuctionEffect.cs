using System.Collections;
using UnityEngine;

public class SuctionEffect : MonoBehaviour
{
    [Header("Suction Settings")]
    public Transform targetPoint;          // The point where objects will be suctioned
    public float suctionDuration = 2f;     // Time for objects to reach the target point
    public AnimationCurve suctionCurve;    // Curve for smoother movement (optional)
    public bool destroyAtTarget = false;   // Option to destroy objects when they reach the target
    public GameObject[] objectsToSuction;  // Objects to suction
    public float noiseIntensity = 0.5f;    // Intensity of the noise for shaky movement
    public float noiseFrequency = 1.5f;    // Frequency of the noise for movement

    private void OnEnable()
    {
        // Start suction as soon as the script is enabled
        if (objectsToSuction != null && objectsToSuction.Length > 0)
        {
            foreach (GameObject obj in objectsToSuction)
            {
                StartCoroutine(SuctionObject(obj));
            }
        }
        else
        {
            Debug.LogWarning("SuctionEffect: No objects assigned for suction.");
        }
    }

    private IEnumerator SuctionObject(GameObject obj)
    {
        Vector3 startPosition = obj.transform.position;
        Vector3 startScale = obj.transform.localScale;
        float elapsedTime = 0f;

        // Generate a random seed for the noise so each object has its own unique movement
        float randomSeedX = Random.Range(0f, 100f);
        float randomSeedY = Random.Range(0f, 100f);
        float randomSeedZ = Random.Range(0f, 100f);

        while (elapsedTime < suctionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / suctionDuration;

            // Apply suction curve if available, otherwise use linear interpolation
            float curveValue = suctionCurve != null ? suctionCurve.Evaluate(t) : t;

            // Add Perlin noise-based random offset for shakiness
            Vector3 noiseOffset = new Vector3(
                Mathf.PerlinNoise(randomSeedX, elapsedTime * noiseFrequency) * 2f - 1f,
                Mathf.PerlinNoise(randomSeedY, elapsedTime * noiseFrequency) * 2f - 1f,
                Mathf.PerlinNoise(randomSeedZ, elapsedTime * noiseFrequency) * 2f - 1f
            ) * noiseIntensity;

            // Interpolate the object's position towards the target
            obj.transform.position = Vector3.Lerp(startPosition, targetPoint.position, curveValue) + noiseOffset;

            // Scale down the object over time
            obj.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, curveValue);

            yield return null;
        }

        // Ensure the object reaches the exact target point and final scale
        obj.transform.position = targetPoint.position;
        obj.transform.localScale = Vector3.zero;

        // Optional: Destroy the object
        if (destroyAtTarget)
        {
            Destroy(obj);
        }
    }
}
