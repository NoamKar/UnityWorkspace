using UnityEngine;

public class StepOutTrigger : MonoBehaviour
{
    public GameObject videoSphere;
    private bool playerInside;

    void Start()
    {
        // Ensure the video sphere starts with its mesh renderer disabled
        videoSphere.GetComponent<MeshRenderer>().enabled = false;
        playerInside = true; // Player starts inside the trigger zone
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            videoSphere.GetComponent<MeshRenderer>().enabled = false; // Ensure it's off when player is inside
            Debug.Log("player is in place");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            videoSphere.GetComponent<MeshRenderer>().enabled = true; // Enable when player steps out
            Debug.Log("player is outside");

        }
    }
}
