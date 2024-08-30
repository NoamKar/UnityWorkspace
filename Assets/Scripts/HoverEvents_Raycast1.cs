using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverEvents_Raycast1 : MonoBehaviour
{
    public GameObject raySource;
    public int rayLength = 100;
    public LayerMask layerMaskInteract;
    public KeyCode ClickKey = KeyCode.Mouse0;

    private bool isMouseEntered = false;
    private bool doOnce = false;

    public UnityEvent MouseEnter;
    public UnityEvent MouseExit;
    public UnityEvent MouseClicking;

    private void Update()
    {
        Vector3 direction = raySource.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(raySource.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            GameObject obj = this.gameObject;
            if (hit.collider.gameObject == obj)
            {
                if (!doOnce)
                {
                    MouseEnter.Invoke();
                }

                isMouseEntered = true;
                doOnce = true;

                if (Input.GetKeyDown(ClickKey))
                {
                    MouseClicking.Invoke();
                    return;
                }
            }
        }
        else
        {
            if (isMouseEntered)
            {
                MouseExit.Invoke();
                GetComponent<Timer_Events>().StopTimerAndReset();  // Stop the timer
                GetComponent<Timer_Events>().GazeExited.Invoke();  // Trigger the GazeExited event
                isMouseEntered = false;
                doOnce = false;
            }
        }
    }
}
