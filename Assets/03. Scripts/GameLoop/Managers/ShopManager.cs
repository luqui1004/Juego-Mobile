using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public int DamagePrice { get; private set; } = 10;
    public int ShieldPrice { get; private set; } = 10;
    public int HealthPrice { get; private set; } = 10;

    [Header("UI Texts")]
    [SerializeField] private TMP_Text damagePriceText;
    [SerializeField] private TMP_Text shieldPriceText;
    [SerializeField] private TMP_Text healthPriceText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdatePriceUI();
    }

    public bool TryBuyDamage()
    {
        if (!ScoreManager.Instance.RemoveCoins(DamagePrice))
            return false;

        ScoreManager.Instance.AddDamage();
        DamagePrice += 2;

        UpdatePriceUI();
        return true;
    }

    public bool TryBuyShield()
    {
        if (!ScoreManager.Instance.RemoveCoins(ShieldPrice))
            return false;

        ScoreManager.Instance.AddShield();
        ShieldPrice += 2;

        UpdatePriceUI();
        return true;
    }

    public bool TryBuyHealth()
    {
        if (!ScoreManager.Instance.RemoveCoins(HealthPrice))
            return false;

        ScoreManager.Instance.RestoreHealth();
        HealthPrice += 2;

        UpdatePriceUI();
        return true;
    }

    private void UpdatePriceUI()
    {
        damagePriceText.text = DamagePrice.ToString();
        shieldPriceText.text = ShieldPrice.ToString();
        healthPriceText.text = HealthPrice.ToString();
    }
}
