using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;

        foreach (var p in FindObjectsOfType<MonoBehaviour>())
        {
            if (p is IPausable pausable)
                pausable.OnPause();
        }
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        foreach (var p in FindObjectsOfType<MonoBehaviour>())
        {
            if (p is IPausable pausable)
                pausable.OnResume();
        }
    }
}
