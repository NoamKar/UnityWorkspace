using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause_Resume : MonoBehaviour
{
    private bool wasclickedfirsttime = false;
    private bool SelectButtonIsPressed = false;

    public InputActionReference PauseResume_ActionMap;

    private void OnEnable()
    {
        PauseResume_ActionMap.action.started += OnToggle;
        PauseResume_ActionMap.action.performed += OnToggle;
        PauseResume_ActionMap.action.canceled += OnToggle;
    }

    private void OnDisable()
    {
        PauseResume_ActionMap.action.started -= OnToggle;
        PauseResume_ActionMap.action.performed -= OnToggle;
        PauseResume_ActionMap.action.canceled -= OnToggle;
    }

    public void OnToggle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SelectButtonIsPressed = false;
        }
        if (context.performed)
        {
            SelectButtonIsPressed = !SelectButtonIsPressed;
            if (SelectButtonIsPressed == true && wasclickedfirsttime == false)
            {
                Time.timeScale = 0f;
                Time.fixedDeltaTime = 0.01388889f * Time.timeScale;
                AudioListener.pause = true;
                wasclickedfirsttime = true;
                return;
            }
            if (SelectButtonIsPressed == true && wasclickedfirsttime == true)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.01388889f * Time.timeScale;
                AudioListener.pause = false;
                wasclickedfirsttime = false;
                return;
            }
        }
        if (context.canceled)
        {
            SelectButtonIsPressed = false;
        }
    }
}
