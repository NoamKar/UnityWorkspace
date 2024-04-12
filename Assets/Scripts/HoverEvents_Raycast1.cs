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
    private bool wasclickedfirsttime = false;    

    public UnityEvent MouseEnter;
    public UnityEvent MouseExit;
    public UnityEvent MouseClicking;
    //public UnityEvent ClickedEvent2;

    private bool mouseenteredbool = false;
    private bool mouseexitedbool = false;
    private bool mouseclickedbool = false;

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            mouseenteredbool = false;
            mouseexitedbool = false;
            mouseclickedbool = false;
        }
        else
        {
            mouseenteredbool = true;
            mouseexitedbool = true;
            mouseclickedbool = true;
        }

        //** for 1ST Person - uncomment out the 2 rows below, and comment out the 3rd row below.       
        //Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));
        //Vector3 direction = worldMousePosition - Camera.main.transform.position;
        Vector3 direction = raySource.transform.TransformDirection(Vector3.forward);
        
        RaycastHit hit;

        if (Physics.Raycast(raySource.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            GameObject obj = this.gameObject;
            if (hit.collider.gameObject == obj)
            {
                if (doOnce == false && mouseenteredbool == true)
                {
                    MouseEnter.Invoke();
                }

                isMouseEntered = true;
                doOnce = true;

                if (Input.GetKeyDown(ClickKey) && wasclickedfirsttime == false && mouseclickedbool == true)
                {
                    MouseClicking.Invoke();
                    //wasclickedfirsttime = true;                    
                    return;
                }

                //if (Input.GetKeyDown(ClickKey) && wasclickedfirsttime == true && mouseclickedbool == true)
                //{
                    //ClickedEvent2.Invoke();
                    //wasclickedfirsttime = false;                    
                    //return;
                //}
            }
        }

        else
        {
            if (isMouseEntered == true && mouseexitedbool == true)
            {
                MouseExit.Invoke();
                doOnce = false;
            }
        }
    }   

}
