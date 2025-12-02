using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;

    private void Start()
    {
        highScoreText.text = "Highscore: " + ScoreManager.LastRunScore;
    }
}
