using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class ReliableVideoLoader : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer;        // Assign your Video Player in the Inspector
    public RenderTexture renderTexture;   // Assign your Render Texture in the Inspector
    public float playDelay = 0.5f;        // Optional delay before playback starts

    void Start()
    {
        // Ensure Render Texture is assigned
        if (renderTexture != null)
        {
            videoPlayer.targetTexture = renderTexture;
            Debug.Log("Render Texture assigned to Video Player.");
        }
        else
        {
            Debug.LogError("Render Texture is missing! Assign a Render Texture to the script.");
            return;
        }

        // Set Video Player properties explicitly
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;

        // Attach event listeners
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.errorReceived += OnVideoError;

        // Start preparing the video
        Debug.Log("Preparing video...");
        StartCoroutine(EnsureVideoPrepared());
    }

    private IEnumerator EnsureVideoPrepared()
    {
        videoPlayer.Prepare();

        // Wait until the video is fully prepared
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Waiting for video to prepare...");
            yield return null;
        }

        Debug.Log("Video is fully prepared.");

        // Optional delay before playback starts
        if (playDelay > 0)
        {
            Debug.Log($"Delaying playback by {playDelay} seconds...");
            yield return new WaitForSeconds(playDelay);
        }

        videoPlayer.Play();
        Debug.Log("Video playback started.");
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video prepared successfully.");
    }

    private void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError($"Video Player Error: {message}");
    }

    void Update()
    {
        // Debugging: Check playback status during runtime
        if (videoPlayer.isPlaying)
        {
            Debug.Log("Video is playing...");
        }
        else if (videoPlayer.isPrepared && !videoPlayer.isPlaying)
        {
            Debug.Log("Video is prepared but not playing.");
        }
    }
}