using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    [Header("Subtitle Tags")]
    public string hebrewTag = "Subtitle_Hebrew";
    public string englishTag = "Subtitle_English";

    private int subtitleState = 0; // 0 = OFF, 1 = Hebrew ON, 2 = English ON (Loops)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CycleSubtitles();
            Debug.Log("s pressed");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySubtitleState();
    }

    private void CycleSubtitles()
    {
        subtitleState = (subtitleState + 1) % 3; // Loops: 0 1 2 ...
        ApplySubtitleState();
    }

    private void ApplySubtitleState()
    {
        List<GameObject> hebrewObjects = FindObjectsWithTag(hebrewTag);
        List<GameObject> englishObjects = FindObjectsWithTag(englishTag);

        if (hebrewObjects.Count == 0 && englishObjects.Count == 0)
        {
            Debug.LogWarning("No subtitle objects found in this scene.");
            return;
        }

        foreach (GameObject obj in hebrewObjects)
        {
            obj.SetActive(subtitleState == 1);
        }

        foreach (GameObject obj in englishObjects)
        {
            obj.SetActive(subtitleState == 2);
        }

        Debug.Log($"Subtitle State: {subtitleState} (0=OFF, 1=Hebrew, 2=English)");
    }

    private List<GameObject> FindObjectsWithTag(string tag)
    {
        List<GameObject> foundObjects = new List<GameObject>();
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>(); // Finds ALL objects, even disabled ones

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag) && obj.hideFlags == HideFlags.None) // Ensures it's not an internal Unity object
            {
                foundObjects.Add(obj);
            }
        }

        return foundObjects;
    }
}
