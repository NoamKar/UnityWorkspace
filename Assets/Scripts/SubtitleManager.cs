using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    [Header("Subtitle Text Objects")]
    public TextMeshProUGUI[] subtitleTexts; // 4 Subtitle Texts

    [Header("Subtitle Configuration")]
    public SubtitleData[] subtitles; // Array of subtitles

    private Coroutine subtitleCoroutine; // Store coroutine for resetting

    private void Start()
    {
        StartSubtitleSequence(); // Auto-start subtitles
    }

    public void StartSubtitleSequence()
    {
        if (subtitleCoroutine != null)
            StopCoroutine(subtitleCoroutine); // Stop previous sequence
        subtitleCoroutine = StartCoroutine(SubtitleSequence());
    }

    private IEnumerator SubtitleSequence()
    {
        float sceneStartTime = Time.time; // Record start time

        foreach (var subtitle in subtitles)
        {
            // Wait until it's time for the subtitle to appear
            float waitTime = subtitle.startTime - (Time.time - sceneStartTime);
            if (waitTime > 0)
                yield return new WaitForSeconds(waitTime);

            // Display subtitle on all 4 locations
            foreach (var textObject in subtitleTexts)
            {
                if (textObject != null)
                {
                    textObject.text = subtitle.text;
                    textObject.enabled = true;
                }
            }

            // Wait for subtitle duration
            yield return new WaitForSeconds(subtitle.duration);

            // Hide the subtitle after its duration
            foreach (var textObject in subtitleTexts)
            {
                if (textObject != null)
                {
                    textObject.text = "";
                    textObject.enabled = false;
                }
            }
        }
    }

    // ** Reset subtitles when restarting**
    public void ResetSubtitles()
    {
        if (subtitleCoroutine != null)
            StopCoroutine(subtitleCoroutine);

        foreach (var textObject in subtitleTexts)
        {
            if (textObject != null)
            {
                textObject.text = "";
                textObject.enabled = false;
            }
        }

        StartSubtitleSequence(); // Restart subtitle sequence
    }
}

//  **Make sure this class is at the end of the script**
[System.Serializable]
public class SubtitleData
{
    public string text;       // Subtitle text
    public float duration;    // Duration (in seconds)
    public float startTime;   // When the subtitle should appear
}