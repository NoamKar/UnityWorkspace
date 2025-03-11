using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CanvasManagerMenu : MonoBehaviour
{
    public static CanvasManagerMenu Instance;

    [Header("Canvas Group Names")]
    public string hebrewCanvasName = "HebrewCanvas";
    public string englishCanvasName = "EnglishCanvas";

    private CanvasGroup hebrewCanvas;
    private CanvasGroup englishCanvas;

    private bool isHebrewVisible = true; // Default state

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

    private void Start()
    {
        FindCanvasGroups();
        ApplyCanvasVisibility();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCanvasGroups();
        ApplyCanvasVisibility();
    }

    private void FindCanvasGroups()
    {
        // Find canvases dynamically in each scene
        hebrewCanvas = FindCanvasGroupByName(hebrewCanvasName);
        englishCanvas = FindCanvasGroupByName(englishCanvasName);
    }

    private CanvasGroup FindCanvasGroupByName(string canvasName)
    {
        GameObject canvasObj = GameObject.Find(canvasName);
        if (canvasObj != null)
        {
            CanvasGroup group = canvasObj.GetComponent<CanvasGroup>();
            if (group != null)
            {
                return group;
            }
        }
        return null;
    }

    public void ToggleCanvasVisibility()
    {
        isHebrewVisible = !isHebrewVisible;
        Debug.Log($"ToggleCanvasVisibility() called. isHebrewVisible is now: {isHebrewVisible}");
        ApplyCanvasVisibility();
    }


    private void ApplyCanvasVisibility()
    {
        if (hebrewCanvas != null)
        {
            hebrewCanvas.alpha = isHebrewVisible ? 1 : 0;
            hebrewCanvas.interactable = isHebrewVisible;
            hebrewCanvas.blocksRaycasts = isHebrewVisible;
        }

        if (englishCanvas != null)
        {
            englishCanvas.alpha = isHebrewVisible ? 0 : 1;
            englishCanvas.interactable = !isHebrewVisible;
            englishCanvas.blocksRaycasts = !isHebrewVisible;
        }
    }
}
