using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health { get; private set; }
    public int Damage { get; private set; }
    public float SpeedArrow { get; private set; }
    public float IntervalArrow { get; private set; }

    [SerializeField] private float moveSpeed = 3f;

    private EnemySpawner spawner;
    private EnemySpawner.EnemyType myType;
    private PlayerController playerController;

    //Animator
    [SerializeField] public Animator anim;

    [SerializeField] public GameObject Coin;

    private void Awake()
    {
        anim = GetComponent<Animator>(); 
    }
    private void Start()
    {
        Coin = GameObject.Find("Coin");
    }

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
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();

            moveSpeed = 0.000000000001f;
            playerController.isInCombat = true;
            CombatController.Instance.SetCombatStats(SpeedArrow, IntervalArrow);

            CombatController.Instance.currentEnemy = this;

            CombatController.Instance.StartCombat();
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
        CombatController.Instance.EndCombat();

        if (playerController != null)
            playerController.isInCombat = false;

        spawner.OnEnemyKilled(myType);

        ScoreManager.Instance.AddScore(100);
        ScoreManager.Instance.AddCoins(50);
        anim.SetTrigger("Death"); //Anim de muerte.
        Instantiate(Coin, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.5f);
    }
}
