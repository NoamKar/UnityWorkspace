using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;                     // The UI Image for fade effect
    public float fadeDuration = 1f;             // Duration of the fade in/out

    [Header("Audio Settings")]
    public AudioSource audioSource;             // The audio source to fade out
    public float audioFadeDuration = 1f;        // Duration to fade out the audio

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // Start fading out audio if an AudioSource is assigned
        if (audioSource != null)
        {
            StartCoroutine(FadeOutAudio());
        }

        // Fade to black
        yield return StartCoroutine(FadeImage(true));

        // Load the new scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Fade from black
        yield return StartCoroutine(FadeImage(false));

        // Optionally fade audio back in (if the same audio source is reused)
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < audioFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / audioFadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop(); // Optionally stop the audio
    }

    private IEnumerator FadeImage(bool fadeToBlack)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        float startAlpha = fadeToBlack ? 0f : 1f;
        float endAlpha = fadeToBlack ? 1f : 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Ensure the final alpha is set
        color.a = endAlpha;
        fadeImage.color = color;
    }
}
