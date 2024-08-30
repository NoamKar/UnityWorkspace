using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GazeInteraction : MonoBehaviour
{
    public GameObject raySource; // Typically the Main Camera
    public int rayLength = 100;
    public LayerMask layerMaskInteract;

    public float gazeDuration = 0.5f; // Time required to trigger the gaze interaction
    private float gazeTimer = 0f;

    public string startHoverTrigger = "Start Hover"; // Animation trigger for starting hover
    public string endHoverTrigger = "End Hover"; // Animation trigger for ending hover

    private bool isGazing = false;
    private bool gazeComplete = false;
    private bool hasExited = false;
    private Animator animator;

    public float exitBufferTime = 0.2f; // Buffer time to confirm the gaze has exited
    private float exitTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>(); // Assuming the Animator is on the same object
    }

    void Update()
    {
        Vector3 direction = raySource.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(raySource.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                if (!isGazing)
                {
                    // Gaze just started
                    Debug.Log("Gaze entered");
                    isGazing = true;
                    gazeTimer = 0f;
                    exitTimer = 0f;
                    hasExited = false; // Reset exit state
                }

                // Increment the gaze timer
                gazeTimer += Time.deltaTime;

                if (gazeTimer >= gazeDuration && !gazeComplete)
                {
                    // Gaze duration complete, trigger start hover animation
                    animator.SetTrigger(startHoverTrigger);
                    gazeComplete = true;
                }
            }
            else
            {
                HandleGazeExit();
            }
        }
        else
        {
            HandleGazeExit();
        }
    }

    void HandleGazeExit()
    {
        if (isGazing && !hasExited)
        {
            // Start exit buffer timer
            exitTimer += Time.deltaTime;

            if (exitTimer >= exitBufferTime)
            {
                // Gaze has truly exited, trigger the end hover animation
                Debug.Log("Gaze exited");
                animator.SetTrigger(endHoverTrigger);
                isGazing = false;
                gazeComplete = false;
                hasExited = true;
            }
        }
    }
}
