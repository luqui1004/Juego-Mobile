using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonAdministration : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        PauseManager.Instance.ResumeGame();
    }

    public void GoToPlayLoop()
    {
        SceneManager.LoadScene(1);
        PauseManager.Instance.ResumeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
