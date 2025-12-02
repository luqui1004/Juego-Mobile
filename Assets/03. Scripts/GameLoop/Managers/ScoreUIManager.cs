using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text coinsText;

    [Header("Stats UI")]
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text shieldText;
    [SerializeField] private Image healthFill;

    private void Start()
    {
        UpdateScore(ScoreManager.Instance.Score);
        UpdateCoins(ScoreManager.Instance.Coins);

        UpdateDamage(ScoreManager.Instance.Damage);
        UpdateShield(ScoreManager.Instance.Shield);
        UpdateHealth(ScoreManager.Instance.Health / 100f);
    }

    public void UpdateScore(int value)
    {
        scoreText.text = "HIGH SCORE: " + value;
    }

    public void UpdateCoins(int value)
    {
        coinsText.text = "COINS: " + value;
    }

    public void UpdateDamage(int value)
    {
        damageText.text = value.ToString();
    }

    public void UpdateShield(int value)
    {
        shieldText.text = value.ToString();
    }

    public void UpdateHealth(float normalizedValue)
    {
        healthFill.fillAmount = normalizedValue;
    }
}
