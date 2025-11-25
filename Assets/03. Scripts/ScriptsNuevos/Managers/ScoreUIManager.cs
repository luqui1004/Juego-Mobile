using UnityEngine;
using TMPro;

public class ScoreUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text coinsText;

    private void Start()
    {
        UpdateScore(ScoreManager.Instance.Score);
        UpdateCoins(ScoreManager.Instance.Coins);
    }

    public void UpdateScore(int value)
    {
        scoreText.text = "HIGH SCORE: " + value;
    }

    public void UpdateCoins(int value)
    {
        coinsText.text = "COINS: " + value;
    }
}
