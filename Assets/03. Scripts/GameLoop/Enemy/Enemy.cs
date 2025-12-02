using UnityEngine;

public class Enemy : MonoBehaviour, IPausable
{
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public float SpeedArrow { get; private set; }
    public float IntervalArrow { get; private set; }

    [SerializeField] private float moveSpeed = 3f;
    public GameObject deathParticles;
    private float savedMoveSpeed;

    private EnemySpawner spawner;
    private EnemySpawner.EnemyType myType;
    private PlayerController playerController;

    private bool isPaused = false;
    public Animator anim; //Sirve para que muera el enemigo

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

    void Update()
    {
        if (isPaused) return;

        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();

            moveSpeed = 0f;
            playerController.isInCombat = true;
            CombatController.Instance.SetCombatStats(SpeedArrow, IntervalArrow);

            CombatController.Instance.currentEnemy = this;

            CombatController.Instance.StartCombat();
        }
    }

    public void TakeDamage()
    {
        Health -= ScoreManager.Instance.Damage;
        TextSpawnManager.Instance.SpawnPerfectText();

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        CombatController.Instance.EndCombat();

        if (playerController != null)
            playerController.isInCombat = false;

        spawner.OnEnemyKilled(myType);

        ScoreManager.Instance.AddScore(100);
        ScoreManager.Instance.AddCoins(50);

        if (deathParticles != null)
        {
            GameObject p = Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(p, 0.5f);
        }
        anim.SetTrigger("Dead");
        Destroy(gameObject);
    }

    public void OnPause()
    {
        isPaused = true;
        savedMoveSpeed = moveSpeed;
        moveSpeed = 0f;
    }

    public void OnResume()
    {
        isPaused = false;
        moveSpeed = savedMoveSpeed;
    }
}
