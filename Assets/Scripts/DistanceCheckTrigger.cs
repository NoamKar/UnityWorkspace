using UnityEngine;
public class DistanceCheckTrigger : MonoBehaviour
{
    public GameObject videoSphere;
    public AlphaTweening videoAlphaTweening;
    public Transform playerTransform;
    public float triggerDistance = 5f; // Adjust this distance as needed
    public ParticleSystem transitionEffect;

    public GameObject[] objectsToDisable; // Array of objects to disable meshes

    private bool playerInside;

    void Start()
    {
        // Ensure the video sphere starts with its mesh renderer disabled
       // videoSphere.GetComponent<MeshRenderer>().enabled = false;
        playerInside = true; // Player starts inside the trigger zone
        Debug.Log("player is in place");
        

    }

    void Update()
    {
        // Calculate the distance between player and trigger zone center
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check if player moves beyond the trigger distance
        if (Mathf.Abs(distanceToPlayer) > triggerDistance)
        {
            if (playerInside)
            {
                // Player has moved outside the trigger zone
           //     videoSphere.GetComponent<MeshRenderer>().enabled = true;
                videoAlphaTweening.SwitchOn();
                playerInside = false;

                foreach (GameObject obj in objectsToDisable)
                {
                    obj.SetActive(false);
                }
                Debug.Log("player is outside");

            }
        }
        else
        {
            if (!playerInside)
            {
                // Player has moved back inside the trigger zone
              //  videoSphere.GetComponent<MeshRenderer>().enabled = false;
                videoAlphaTweening.SwitchOff();
                playerInside = true;
                foreach (GameObject obj in objectsToDisable)
                {
                    obj.SetActive(true);
                }
                Debug.Log("player is in place");

            }
        }
    }
}
