using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer_Events : MonoBehaviour
{
    public float initialTime = 10f;  // Set the initial time in the Inspector
    public float timeRemaining;
    public bool timerIsRunning = false;
    public UnityEvent TimeEnded;
    public UnityEvent secondEvent_TimeEnded;
    public UnityEvent GazeExited;  // This event will be invoked when the gaze exits the object
    public float secondEventTimeDelayed;

    private void Start()
    {
        timeRemaining = initialTime;  // Initialize the timeRemaining with initialTime
        Debug.Log("Timer starts with " + timeRemaining + " seconds.");
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }

    public void StopTimerAndReset()
    {
        timerIsRunning = false;
        timeRemaining = initialTime;  // Reset to the initial time
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                TimeEnded.Invoke();
                StartCoroutine(SecondEventDelay());
            }
        }
    }

    IEnumerator SecondEventDelay()
    {
        yield return new WaitForSeconds(secondEventTimeDelayed);
        secondEvent_TimeEnded.Invoke();
    }

    //void HandleGazeExit()
    //{
        //animator.SetTrigger("End Hover");
        //// Optionally set a boolean or trigger for looping back
        //animator.SetTrigger("Loop"); // Use this in Animator to transition back to Idle
    //}

}
