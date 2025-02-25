using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;

public class ExperienceManager : MonoBehaviour
{
    public string firstSceneName = "Scene01"; // Set this to the name of your first scene
    public AudioSource backgroundAudio; // Persistent audio (Don't Destroy On Load)
    private bool isPaused = false; // Pause state tracking

    private List<AudioSource> audioSourcesPlaying = new List<AudioSource>(); // Fix: Declare at the class level


    private void Awake()
    {
        if (FindObjectsOfType<ExperienceManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartExperience();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ExitExperience();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void RestartExperience()
    {
        Debug.Log("Restarting Experience...");

        if (backgroundAudio != null)
        {
            backgroundAudio.Stop();
            backgroundAudio.time = 0;
            backgroundAudio.Play();
        }

        PlayableDirector timeline = FindObjectOfType<PlayableDirector>();
        if (timeline != null)
        {
            timeline.time = 0;
            timeline.Play();
        }

        SubtitleManager subtitleManager = FindObjectOfType<SubtitleManager>();
        if (subtitleManager != null)
        {
            subtitleManager.ResetSubtitles();
        }

        // **Ensure Scene Transition Manager exists after restart**
        GameObject newTransitionManager = new GameObject("SceneTransitionManager");
        newTransitionManager.AddComponent<SceneTransitionManager>();

        SceneManager.LoadScene(firstSceneName);
    }


    public void ExitExperience()
    {
        Debug.Log("Exiting Experience...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log($"Pause toggled: {isPaused}");

        if (isPaused)
        {
            Time.timeScale = 0; // Pause game logic

            // **Track which audio sources were playing**
            audioSourcesPlaying.Clear(); // Reset list

            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in allAudioSources)
            {
                if (audio.isPlaying) // **Only store active audio sources**
                {
                    audioSourcesPlaying.Add(audio);
                    audio.Pause();
                    Debug.Log($"Paused: {audio.gameObject.name}");
                }
            }

            if (backgroundAudio != null && backgroundAudio.isPlaying)
            {
                backgroundAudio.Pause();
                audioSourcesPlaying.Add(backgroundAudio); // Track it separately
                Debug.Log("Background audio paused.");
            }

            // Pause Timeline
            PlayableDirector timeline = FindObjectOfType<PlayableDirector>();
            if (timeline != null) timeline.Pause();

            // Pause Video Players
            GameObject[] videoPlayers = GameObject.FindGameObjectsWithTag("VideoPlayer");
            foreach (GameObject videoObject in videoPlayers)
            {
                VideoPlayer videoPlayer = videoObject.GetComponent<VideoPlayer>();
                if (videoPlayer != null) videoPlayer.Pause();
            }

            Debug.Log("Experience Paused.");
        }
        else
        {
            Time.timeScale = 1; // Resume game logic

            // **Only resume audio sources that were playing before pause**
            foreach (AudioSource audio in audioSourcesPlaying)
            {
                if (audio != null) // Ensure the object wasn't destroyed
                {
                    audio.Play();
                    Debug.Log($"Resumed: {audio.gameObject.name}");
                }
            }
            audioSourcesPlaying.Clear(); // Clear the list

            // Resume Timeline
            PlayableDirector timeline = FindObjectOfType<PlayableDirector>();
            if (timeline != null) timeline.Play();

            // Resume Video Players
            GameObject[] videoPlayers = GameObject.FindGameObjectsWithTag("VideoPlayer");
            foreach (GameObject videoObject in videoPlayers)
            {
                VideoPlayer videoPlayer = videoObject.GetComponent<VideoPlayer>();
                if (videoPlayer != null) videoPlayer.Play();
            }

            Debug.Log("Experience Resumed.");
        }
    }

}