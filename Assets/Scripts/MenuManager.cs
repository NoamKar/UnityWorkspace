using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Video;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public string firstSceneName = "Scene01"; // Set this to the name of your first scene
    public AudioSource backgroundAudio; // Persistent audio (Don't Destroy On Load)
    private bool isPaused = false; // Pause state tracking

    private void Awake()
    {
        if (FindObjectsOfType<ExperienceManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    //private void Update()
    //{
        //if (Input.GetKeyDown(KeyCode.R))
        //{
            //RestartExperience();
        //}

        //if (Input.GetKeyDown(KeyCode.X))
        //{
            //ExitExperience();
        //}

        //if (Input.GetKeyDown(KeyCode.P))
        //{
            //TogglePause();
        //}
    //}

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

        SceneTransitionDissolve sceneLoader = FindObjectOfType<SceneTransitionDissolve>();
        if (sceneLoader != null)
        {
            sceneLoader.ResetPreloadStatus();  // Reset scene transition system
        }

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

            // Pause background audio if it exists
            if (backgroundAudio != null)
            {
                backgroundAudio.Pause();
                Debug.Log("Background audio paused.");
            }

            // Pause ALL 3D AudioSources
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in allAudioSources)
            {
                if (audio != backgroundAudio) // Avoid pausing the persistent background audio again
                {
                    audio.Pause();
                    Debug.Log($"Paused: {audio.gameObject.name}");
                }
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

            // Resume background audio if it exists
            if (backgroundAudio != null)
            {
                backgroundAudio.Play();
                Debug.Log("Background audio resumed.");
            }

            // Resume ALL 3D AudioSources
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in allAudioSources)
            {
                if (audio != backgroundAudio) // Avoid resuming persistent background audio again
                {
                    audio.Play();
                    Debug.Log($"Resumed: {audio.gameObject.name}");
                }
            }

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
