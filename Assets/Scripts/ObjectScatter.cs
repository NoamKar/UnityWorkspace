using UnityEngine;

public class ScatterChildObjects : MonoBehaviour
{
    public Transform player;            // Reference to the player or camera
    public float scatterDistance = 10f; // Distance to move the objects
    public float scatterSpeed = 2f;     // Speed of the scatter animation
    public bool affectX = true;         // Should movement affect the X-axis?
    public bool affectY = false;        // Should movement affect the Y-axis?
    public bool affectZ = true;         // Should movement affect the Z-axis?

    private Vector3[] originalPositions; // Store original positions of child objects
    private Transform[] childTransforms; // Store child transforms

    private bool isScattering = false;   // Track if scattering is in progress

    void Start()
    {
        // Get all child objects of this parent
        int childCount = transform.childCount;
        childTransforms = new Transform[childCount];
        originalPositions = new Vector3[childCount];

        for (int i = 0; i < childCount; i++)
        {
            childTransforms[i] = transform.GetChild(i);
            originalPositions[i] = childTransforms[i].position;
        }
    }

    public void TriggerScatter()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing! Assign a player Transform in the Inspector.");
            return;
        }

        isScattering = true;
    }

    void Update()
    {
        if (isScattering)
        {
            ScatterObjects();
        }
    }

    private void ScatterObjects()
    {
        bool allObjectsScattered = true;

        for (int i = 0; i < childTransforms.Length; i++)
        {
            Transform child = childTransforms[i];

            // Calculate direction away from the player for each child
            Vector3 direction = (child.position - player.position).normalized;

            // Apply axis filters to the direction
            direction = new Vector3(
                affectX ? direction.x : 0f,
                affectY ? direction.y : 0f,
                affectZ ? direction.z : 0f
            );

            Vector3 targetPosition = originalPositions[i] + direction * scatterDistance;

            // Move the child object towards the target position
            child.position = Vector3.MoveTowards(child.position, targetPosition, scatterSpeed * Time.deltaTime);

            // Check if the object has reached its target position
            if (Vector3.Distance(child.position, targetPosition) > 0.01f)
            {
                allObjectsScattered = false;
            }
        }

        // Stop scattering when all objects have reached their target positions
        if (allObjectsScattered)
        {
            isScattering = false;
            Debug.Log("All objects have scattered.");
        }
    }
}
