using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Timer_Events : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    //public GameObject TimeText;
    public UnityEvent TimeEnded;
    public UnityEvent secondEvent_TimeEnded;
    public float secondEventTimeDelayed;

    private void Start()
    {
        Debug.Log("timer starts");
    }
    public void StartTimer()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning == true)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                TimeEnded.Invoke();
                StartCoroutine("SecondEventDelay");
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        //TimeText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator SecondEventDelay()
    {
        yield return new WaitForSeconds(secondEventTimeDelayed);

        secondEvent_TimeEnded.Invoke();

    }
}