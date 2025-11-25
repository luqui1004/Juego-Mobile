using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy normalPrefab;
    public Enemy tankPrefab;
    public Enemy runnerPrefab;

    public EnemyStatsBase normalStats;
    public EnemyStatsBase tankStats;
    public EnemyStatsBase runnerStats;

    [Header("DEBUG - Normal Scaling")]
    [SerializeField] private int normalExtraHealth;
    [SerializeField] private int normalExtraDamage;
    [SerializeField] private float normalExtraSpeed;
    [SerializeField] private float normalExtraInterval;

    [Header("DEBUG - Tank Scaling")]
    [SerializeField] private int tankExtraHealth;
    [SerializeField] private int tankExtraDamage;
    [SerializeField] private float tankExtraSpeed;
    [SerializeField] private float tankExtraInterval;

    [Header("DEBUG - Runner Scaling")]
    [SerializeField] private int runnerExtraHealth;
    [SerializeField] private int runnerExtraDamage;
    [SerializeField] private float runnerExtraSpeed;
    [SerializeField] private float runnerExtraInterval;

    void Start()
    {
        SpawnNextEnemy(EnemyType.Normal);
    }

    public enum EnemyType { Normal, Tank, Runner }

    public void OnEnemyKilled(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Normal:
                normalExtraHealth += 50;
                normalExtraDamage += 2;
                normalExtraSpeed += 200f;
                normalExtraInterval += 0.2f;
                break;

            case EnemyType.Tank:
                tankExtraHealth += 50;
                tankExtraDamage += 2;
                tankExtraSpeed += 200f;
                tankExtraInterval += 0.2f;
                break;

            case EnemyType.Runner:
                runnerExtraHealth += 50;
                runnerExtraDamage += 2;
                runnerExtraSpeed += 200f;
                runnerExtraInterval += 0.2f;
                break;
        }

        EnemyType next = (EnemyType)Random.Range(0, 3);
        SpawnNextEnemy(next);
    }

    public void SpawnNextEnemy(EnemyType type)
    {
        Enemy prefab = normalPrefab;
        EnemyStatsBase stats = normalStats;

        int extraHealth = 0;
        int extraDamage = 0;
        float extraSpeed = 0;
        float extraInterval = 0;

        switch (type)
        {
            case EnemyType.Tank:
                prefab = tankPrefab;
                stats = tankStats;

                extraHealth = tankExtraHealth;
                extraDamage = tankExtraDamage;
                extraSpeed = tankExtraSpeed;
                extraInterval = tankExtraInterval;
                break;

            case EnemyType.Runner:
                prefab = runnerPrefab;
                stats = runnerStats;

                extraHealth = runnerExtraHealth;
                extraDamage = runnerExtraDamage;
                extraSpeed = runnerExtraSpeed;
                extraInterval = runnerExtraInterval;
                break;

            case EnemyType.Normal:
                prefab = normalPrefab;
                stats = normalStats;

                extraHealth = normalExtraHealth;
                extraDamage = normalExtraDamage;
                extraSpeed = normalExtraSpeed;
                extraInterval = normalExtraInterval;
                break;
        }

        Enemy newEnemy = Instantiate(prefab, transform.position, Quaternion.identity);
        newEnemy.Init(stats, extraHealth, extraDamage, extraSpeed, extraInterval, this, type);
    }
}
