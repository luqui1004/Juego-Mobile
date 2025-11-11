using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("02. MainLevel");
    }

    // Sale del juego
    public void ExitGame()
    {
        Application.Quit();
    }
}