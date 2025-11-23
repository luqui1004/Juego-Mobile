using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int points = 0;
    [SerializeField] int coins = 0;
    [SerializeField] TextMeshProUGUI Scoreboard;
    [SerializeField] TextMeshProUGUI Coinsboard;
    [Header("ScoreboardPoints")]
    [SerializeField] public int AddPoints;
    [SerializeField] public int AddCoins;
    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        coins = 0;
    }
    private void Update()
    {
        Scoreboard.text = "HIGH SCORE:" + points.ToString();
        Coinsboard.text = "COINS:"+coins.ToString();
    }
    public void AddScore(int amount,int Coins)
    {
        points += amount;
        coins += Coins;
    }
    public void LoseCoins(int amount) {  coins -= amount; }
}
