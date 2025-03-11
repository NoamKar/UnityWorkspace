using UnityEngine;

public class PersistentPause : MonoBehaviour
{
    private static PersistentPause instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
}
