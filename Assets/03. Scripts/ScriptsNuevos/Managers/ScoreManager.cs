using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Coins { get; private set; } = 10;
    public int Score { get; private set; }

    public int Damage { get; private set; } = 1;
    public int Shield { get; private set; } = 1;
    public int Health { get; private set; } = 10;

    private ScoreUIManager ui;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        ui = FindObjectOfType<ScoreUIManager>();
    }

    // SCORE
    public void AddScore(int amount)
    {
        Score += amount;
        ui?.UpdateScore(Score);
    }

    public void RemoveScore(int amount)
    {
        Score -= amount;
        if (Score < 0) Score = 0;

        ui?.UpdateScore(Score);
    }

    public void ResetScore()
    {
        Score = 0;
        ui?.UpdateScore(Score);
    }

    // COINS
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

    // STATS
    public void AddDamage()
    {
        Damage++;
        ui?.UpdateDamage(Damage);
    }

    public void AddShield()
    {
        Shield++;
        ui?.UpdateShield(Shield);
    }

    public void RestoreHealth()
    {
        Health = 10;
        ui?.UpdateHealth(Health / 10f);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0)
        {
            Health = 0;
            SceneManager.LoadScene("03. MainLose");
        }

        ui?.UpdateHealth(Health / 10f);
    }
}
