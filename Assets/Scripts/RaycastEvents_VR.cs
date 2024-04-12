using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RaycastEvents_VR : MonoBehaviour
{
    public int rayLength = 15;

    public InputActionReference GazeSelect_ActionMap;

    private bool isGazed = false;
    private bool doOnce = false;
    private bool wasclickedfirsttime = false;
    private bool SelectButtonIsPressed = false;

    public UnityEvent GazeEnter;
    public UnityEvent GazeExit;
    public UnityEvent ControllerClicked1;
    public UnityEvent ControllerClicked2;

    public void OnToggle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SelectButtonIsPressed = false;
        }
        if (context.performed)
        {
            SelectButtonIsPressed = !SelectButtonIsPressed;
            if (SelectButtonIsPressed == true && isGazed == true && wasclickedfirsttime == false)
            {
                ControllerClicked1.Invoke();
                wasclickedfirsttime = true;
                return;
            }
            if (SelectButtonIsPressed == true && isGazed == true && wasclickedfirsttime == true)
            {
                ControllerClicked2.Invoke();
                wasclickedfirsttime = false;
                return;
            }
        }
        if (context.canceled)
        {
            SelectButtonIsPressed = false;
        }
    }

    private void LateUpdate()
    {
        Vector3 direction = Camera.main.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, rayLength))
        {
            GameObject obj = this.gameObject;
            if (hit.collider.gameObject == obj)
            {
                if (doOnce == false && isGazed == false)
                {
                    GazeEnter.Invoke();
                    isGazed = true;
                    doOnce = true;
                    GazeSelect_ActionMap.action.started += OnToggle;
                    GazeSelect_ActionMap.action.performed += OnToggle;
                    GazeSelect_ActionMap.action.canceled += OnToggle;
                }
            }

            else
            {
                if (doOnce == true && isGazed == true)
                {
                    GazeExit.Invoke();
                    isGazed = false;
                    doOnce = false;
                    GazeSelect_ActionMap.action.started -= OnToggle;
                    GazeSelect_ActionMap.action.performed -= OnToggle;
                    GazeSelect_ActionMap.action.canceled -= OnToggle;
                }
            }
        }        
    }
}

