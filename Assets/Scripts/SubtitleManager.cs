using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    [Header("Subtitle Text Objects")]
    public TextMeshProUGUI[] subtitleTexts; // Array of TextMeshPro objects (for the 4 subtitle locations)

    [Header("Subtitle Configuration")]
    public SubtitleData[] subtitles; // Array of subtitles with text, duration, and start time

    private void Start()
    {
        // Start the subtitle sequence automatically
        StartCoroutine(SubtitleSequence());
    }

    private IEnumerator SubtitleSequence()
    {
        float sceneStartTime = Time.time; // Record when the scene starts

        foreach (var subtitle in subtitles)
        {
            // Calculate the time to wait until this subtitle should appear
            float waitTime = subtitle.startTime - (Time.time - sceneStartTime);
            if (waitTime > 0)
            {
                Debug.Log($"Waiting {waitTime} seconds to display subtitle: {subtitle.text}");
                yield return new WaitForSeconds(waitTime);
            }

            // Display the subtitle on all text objects
            foreach (var textObject in subtitleTexts)
            {
                if (textObject != null)
                {
                    textObject.text = subtitle.text; // Update subtitle text
                    textObject.enabled = true;      // Show the subtitle
                }
            }

            Debug.Log($"Displaying subtitle: {subtitle.text} for {subtitle.duration} seconds");

            // Wait for the duration of the subtitle
            yield return new WaitForSeconds(subtitle.duration);

            // Clear the text on all subtitle locations
            foreach (var textObject in subtitleTexts)
            {
                if (textObject != null)
                {
                    textObject.text = "";           // Clear subtitle text
                    textObject.enabled = false;     // Hide the subtitle
                }
            }

            Debug.Log($"Finished displaying subtitle: {subtitle.text}");
        }

        Debug.Log("All subtitles finished.");
    }
}

[System.Serializable]
public class SubtitleData
{
    public string text;       // Subtitle text
    public float duration;    // Duration to display this subtitle (in seconds)
    public float startTime;   // Time when this subtitle should appear (absolute, in seconds from scene start)
}
