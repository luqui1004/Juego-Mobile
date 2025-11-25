//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class StoreManager : MonoBehaviour
//{
//    [SerializeField] GameObject Store;
//    private PlayerInputs player;
//    private PlayerHealth PlayerHealth;
//    private ScoreManager ScoreManager;
//    [SerializeField] int Cost1;
//    [SerializeField] int Cost2;
//    [SerializeField] int Cost3;

//    void Start()
//    {
//        Store.SetActive(false);
//        player = FindObjectOfType<PlayerInputs>();
//        PlayerHealth = FindObjectOfType<PlayerHealth>();
//        ScoreManager = FindObjectOfType<ScoreManager>();
//    }
//    //private void Update()
//    //{
//    //    Input.GetKeyDown(KeyCode.W);
//    //    { OpenStore(); }
//    //}

//    public void OpenStore()
//    {
//        Store.SetActive(true);
//    }
//    public void PowerUpp1() 
//    {
//        player.Damage += 1;
//        ScoreManager.LoseCoins(Cost1);
//    }
//    public void PowerUpp2() 
//    {
//        player.DamageEnemy -=0.5f;
//        ScoreManager.LoseCoins(Cost2);
//    }
//    public void PowerUpp3() 
//    {
//        PlayerHealth.health += 10;
//        ScoreManager.LoseCoins(Cost3);
//    }
//}
