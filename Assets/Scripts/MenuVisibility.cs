using UnityEngine;

public class MenuVisibility : MonoBehaviour
{
    public CanvasGroup menuCanvasGroup; // Assign in Inspector

    public void ShowMenu()
    {
        menuCanvasGroup.alpha = 1;  // Fully visible
        menuCanvasGroup.interactable = true;
        menuCanvasGroup.blocksRaycasts = true;
    }

    public void HideMenu()
    {
        menuCanvasGroup.alpha = 0;  // Fully invisible
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;
    }
}
