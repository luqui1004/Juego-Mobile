using UnityEngine;
using System.Collections;

public class EnemyFinal : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector2 moveDirection = Vector2.left;

    [Header("Enemy Health")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("References Combat")]
    private GameObject combatUI;
    private ArrowSpawner arrowSpawner;
    private PlayerInputs playerInputs;
    private SpawnerEnemy spawnerEnemy;
    private ParallaxLooper parallaxLooper;
    private Camera mainCamera;

    private bool combatStarted = false;
    private bool isMoving = true;

    private Vector3 originalCameraPosition;
    private float originalCameraSize;

    [Header("Zoom Config")]
    [SerializeField] private float zoomSize = 4.3f;
    [SerializeField] private float zoomDuration = 0.8f;
    [SerializeField] private Vector3 zoomOffset = new Vector3(0f, -0.5f, 0f);

    void Start()
    {
        currentHealth = maxHealth;

        combatUI = GameObject.Find("COMBAT");
        if (combatUI != null)
            combatUI.SetActive(false);

        GameObject spawnerObj = GameObject.Find("SpawnerArrow");
        if (spawnerObj != null)
            arrowSpawner = spawnerObj.GetComponent<ArrowSpawner>();

        GameObject playerObj = GameObject.Find("MainCharacter");
        if (playerObj != null)
            playerInputs = playerObj.GetComponent<PlayerInputs>();

        GameObject spawnerEnemyObj = GameObject.Find("EnemySpawnerPoint");
        if (spawnerEnemyObj != null)
            spawnerEnemy = spawnerEnemyObj.GetComponent<SpawnerEnemy>();

        parallaxLooper = FindObjectOfType<ParallaxLooper>();

        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalCameraPosition = mainCamera.transform.position;
            originalCameraSize = mainCamera.orthographicSize;
        }
    }

    void Update()
    {
        if (isMoving)
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !combatStarted)
            StartCombat();
    }

    void StartCombat()
    {
        combatStarted = true;
        isMoving = false;

        if (combatUI != null)
            combatUI.SetActive(true);

        if (arrowSpawner != null)
            arrowSpawner.enabled = true;

        if (playerInputs != null)
        {
            playerInputs.enabled = true;
            playerInputs.SetIdle(true);
        }

        if (parallaxLooper != null)
        {
            parallaxLooper.stopParallax = true;
        }

        if (mainCamera != null)
            StartCoroutine(CameraZoomIn());

        CombatManager.Instance.StartCombat();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            StartCoroutine(DieSequence());
    }

    IEnumerator DieSequence()
    {
        DieLogic();

        if (mainCamera != null)
            yield return StartCoroutine(CameraZoomOut());

        mainCamera.orthographicSize = originalCameraSize;
        mainCamera.transform.position = originalCameraPosition;

        Destroy(gameObject);
    }

    void DieLogic()
    {
        if (combatUI != null)
            combatUI.SetActive(false);

        if (arrowSpawner != null)
            arrowSpawner.StopSpawning();

        CombatManager.Instance.EndCombat();

        if (parallaxLooper != null)
        {
            parallaxLooper.stopParallax = false;
        }

        if (playerInputs != null)
            playerInputs.SetRunning(true);

        if (spawnerEnemy != null)
            spawnerEnemy.SpawnEnemy();
    }

    public void ReiniciarReferencias()
    {
        combatUI = GameObject.Find("COMBAT");
        if (combatUI != null)
            combatUI.SetActive(false);

        GameObject spawnerObj = GameObject.Find("SpawnerArrow");
        if (spawnerObj != null)
            arrowSpawner = spawnerObj.GetComponent<ArrowSpawner>();

        GameObject playerObj = GameObject.Find("MainCharacter");
        if (playerObj != null)
            playerInputs = playerObj.GetComponent<PlayerInputs>();

        GameObject spawnerEnemyObj = GameObject.Find("EnemySpawnerPoint");
        if (spawnerEnemyObj != null)
            spawnerEnemy = spawnerEnemyObj.GetComponent<SpawnerEnemy>();

        parallaxLooper = FindObjectOfType<ParallaxLooper>();

        Debug.Log("♻️ Referencias de enemigo reiniciadas correctamente.");
    }

    IEnumerator CameraZoomIn()
    {
        float elapsed = 0f;
        float startSize = mainCamera.orthographicSize;
        Vector3 startPos = mainCamera.transform.position;
        Vector3 targetPos = originalCameraPosition + zoomOffset;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / zoomDuration);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, zoomSize, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
    }

    IEnumerator CameraZoomOut()
    {
        float elapsed = 0f;
        float startSize = mainCamera.orthographicSize;
        Vector3 startPos = mainCamera.transform.position;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / zoomDuration);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, originalCameraSize, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, originalCameraPosition, t);
            yield return null;
        }

        mainCamera.orthographicSize = originalCameraSize;
        mainCamera.transform.position = originalCameraPosition;
    }
}
