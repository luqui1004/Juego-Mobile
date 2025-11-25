using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Coins { get; private set; }
    public int Score { get; private set; }

    private ScoreUIManager ui;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ui = FindObjectOfType<ScoreUIManager>();
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        ui?.UpdateCoins(Coins);
    }

    public bool RemoveCoins(int amount)
    {
        if (Coins < amount)
            return false;

        Coins -= amount;
        ui?.UpdateCoins(Coins);

        return true;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        ui?.UpdateScore(Score);
    }

    public void RemoveScore(int amount)
    {
        Score -= amount;
        if (Score < 0)
            Score = 0;

        ui?.UpdateScore(Score);
    }

    public void ResetScore()
    {
        Score = 0;
        ui?.UpdateScore(Score);
    }
}
