using UnityEngine;

public class WireControl : MonoBehaviour
{
    public GameObject raySource;  // The object emitting the ray (e.g., Main Camera)
    public GameObject wireObject; // The wire object with the animation
    public int rayLength = 15;    // Length of the ray
    public LayerMask layerMaskInteract; // Assign appropriate layers to avoid false hits
    public float gazeExitBufferTime = 0.2f;  // Time to wait before triggering GazeExit

    private bool isGazed = false;
    private float exitTimer = 0f;
    private Animator wireAnimator;
    private bool hasPlayedOnce = false; // Track whether the animation has played once

    void Start()
    {
        // Get the Animator component on the wire object
        wireAnimator = wireObject.GetComponent<Animator>();

        // Ensure the wire is inactive at the start
        wireObject.SetActive(false);
    }

    void Update()
    {
        // Create a forward direction based on the ray source's transform
        Vector3 direction = raySource.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        Debug.DrawRay(raySource.transform.position, direction * rayLength, Color.red);

        // Cast a ray from the ray source
        if (Physics.Raycast(raySource.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                if (!isGazed)
                {
                    // Gaze entered
                    GazeEnter();
                    isGazed = true;
                    exitTimer = 0f;  // Reset the exit timer
                }
                else
                {
                    // Reset the exit timer if still gazing at the object
                    exitTimer = 0f;
                }
            }
            else
            {
                // If the ray was previously hitting the object but no longer is, start the exit timer
                exitTimer += Time.deltaTime;
                if (exitTimer >= gazeExitBufferTime)
                {
                    GazeExit();
                }
            }
        }
        else
        {
            // If the ray was previously hitting the object but no longer is, start the exit timer
            exitTimer += Time.deltaTime;
            if (exitTimer >= gazeExitBufferTime)
            {
                GazeExit();
            }
        }
    }

    private void GazeEnter()
    {
        Debug.Log("Gaze Enter: " + this.gameObject.name);

        // Activate the wire object
        wireObject.SetActive(true);

        if (!hasPlayedOnce)
        {
            // Play the growing animation if it hasn't been played yet
            wireAnimator.Play("WireGrowing");
        }
        else
        {
            // Show the final state if the animation has already played
            wireAnimator.Play("WireFinalState", 0, 1f); // Play the final animation state and jump to its last frame
        }
    }

    private void GazeExit()
    {
        Debug.Log("Gaze Exit: " + this.gameObject.name);
        wireObject.SetActive(false);
    }

    // Call this method when the animation finishes (e.g., via an Animation Event in the Animator)
    public void OnAnimationFinished()
    {
        hasPlayedOnce = true;
    }
}
