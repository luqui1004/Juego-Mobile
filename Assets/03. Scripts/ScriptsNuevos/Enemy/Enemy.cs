using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public float SpeedArrow { get; private set; }
    public float IntervalArrow { get; private set; }

    [SerializeField] private float moveSpeed = 3f;

    private EnemySpawner spawner;
    private EnemySpawner.EnemyType myType;

    public void Init(
        EnemyStatsBase stats,
        int extraHealth,
        int extraDamage,
        float extraSpeed,
        float extraInterval,
        EnemySpawner ownerSpawner,
        EnemySpawner.EnemyType type)
    {
        spawner = ownerSpawner;
        myType = type;

        Health = stats.baseHealth + extraHealth;
        Damage = stats.damage + extraDamage;
        SpeedArrow = stats.baseSpeedArrow + extraSpeed;
        IntervalArrow = Mathf.Max(0.1f, stats.baseIntervalArrow - extraInterval);
    }

    //void Start()
    //{
    //    // TEST: matar después de 2s
    //    Invoke(nameof(Die), 2f);
    //}

    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            moveSpeed = 0f;
            //iniciarCombate
        }
    }

    public void TakeDamage() 
    { 
        Health -= ScoreManager.Instance.Damage;
        if (Health <= 0)
        {
            Die();
        }
            
    }

    private void Die()
    {
        //terminarcombate
        spawner.OnEnemyKilled(myType);
        Destroy(gameObject);
    }
}
