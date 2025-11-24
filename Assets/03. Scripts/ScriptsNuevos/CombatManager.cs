using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Combat State")]
    public bool isInCombat = false;
    public bool combatFinished = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartCombat()
    {
        isInCombat = true;
        combatFinished = false;
    }

    public void EndCombat()
    {
        isInCombat = false;
        combatFinished = true;
    }
}

