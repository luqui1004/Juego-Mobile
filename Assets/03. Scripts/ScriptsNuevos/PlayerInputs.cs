using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float tapMaxTime = 0.2f;
    [SerializeField] private float minSwipeDistance = 100;
    [SerializeField] public int Damage = 1;
    [SerializeField] public float DamageEnemy = 1f;

    [Header("Player")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Animator playerAnimator;

    [Header("Arrows")]
    [SerializeField] private ArrowSpawner arrowSpawner;

    [Header("HitZone")]
    [SerializeField] private RectTransform hitZone;

    private float touchStartTime;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    [Header("Feedback Visual")]
    [SerializeField] private GameObject perfectTextPrefab;
    [SerializeField] private GameObject badTextPrefab;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake cameraShake;
    [SerializeField, Range(0f, 1f)] private float shakeIntensity = 0.2f;
    [SerializeField] private float shakeDuration = 0.2f;

    private Canvas mainCanvas;
    private ScoreManager scoreManager;
    [SerializeField] public PlayerHealth playerHealth;


    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        mainCanvas = FindObjectOfType<Canvas>();
        playerHealth= FindObjectOfType<PlayerHealth>();

        if (playerAnimator != null)
            SetRunning(true);

        if (cameraShake == null)
            cameraShake = FindObjectOfType<CameraShake>();
    }

    void Update()
    {
        if (CombateManager.Instance != null && !CombateManager.Instance.isInCombat)
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchStartTime = Time.time;
            startTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            DetectInput();
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartTime = Time.time;
                    startTouchPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPos = touch.position;
                    DetectInput();
                    break;
            }
        }
#endif
    }

    void DetectInput()
    {
        float touchDuration = Time.time - touchStartTime;
        Vector2 swipeDelta = endTouchPos - startTouchPos;

        if (touchDuration <= tapMaxTime && swipeDelta.magnitude < minSwipeDistance)
            return;

        if (swipeDelta.magnitude >= minSwipeDistance)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(y) > Mathf.Abs(x))
            {
                if (y > 0) Swipe(ArrowUI.ArrowDirection.Up);
                else Swipe(ArrowUI.ArrowDirection.Down);
            }
            else
            {
                if (x > 0) Swipe(ArrowUI.ArrowDirection.Right);
                else Swipe(ArrowUI.ArrowDirection.Left);
            }
        }
    }

    void Swipe(ArrowUI.ArrowDirection direction)
    {
        if (arrowSpawner == null) return;

        ArrowUI arrow = arrowSpawner.GetClosestArrow();
        if (arrow == null)
        {
            Debug.Log("No hay flecha activa");
            return;
        }

        if (arrow.Direction == direction)
        {
            bool isInsideHitZone = RectTransformUtility.RectangleContainsScreenPoint(
                hitZone,
                RectTransformUtility.WorldToScreenPoint(null, arrow.transform.position)
            );

            if (isInsideHitZone)
            {
                ShowFeedback("Perfect");
                //scoreManager.AddScore(scoreManager.AddPoints, scoreManager.AddCoins);
                EnemyFinal enemy = FindObjectOfType<EnemyFinal>();

                if (enemy != null)
                    enemy.TakeDamage(Damage);
            }
            else
            {
                ShowFeedback("Bad");
                playerHealth.TakeDamage(DamageEnemy);
                DoCameraShake();
            }

            arrow.MarkAsSwiped();
            Destroy(arrow.gameObject);
        }
        else
        {
            ShowFeedback("Bad");
            playerHealth.TakeDamage(DamageEnemy);
            DoCameraShake();
        }
    }

    public void ShowFeedback(string type)
    {
        if (mainCanvas == null) mainCanvas = FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            return;
        }

        GameObject prefabToSpawn = null;

        if (type == "Perfect") prefabToSpawn = perfectTextPrefab;
        else if (type == "Bad") prefabToSpawn = badTextPrefab;

        if (prefabToSpawn == null)
        {
            return;
        }

        GameObject feedbackInstance = Instantiate(prefabToSpawn, mainCanvas.transform);
        feedbackInstance.transform.SetAsLastSibling();

        Destroy(feedbackInstance, 2f);
    }

    private void DoCameraShake()
    {
        if (cameraShake != null && shakeIntensity > 0f)
            cameraShake.Shake(shakeIntensity, shakeDuration);
    }

    public void SetIdle(bool state)
    {
        if (playerAnimator == null) return;
        playerAnimator.Play(state ? "Idle" : "RunMainCharacter");
    }

    public void SetRunning(bool state)
    {
        if (playerAnimator == null) return;
        playerAnimator.Play(state ? "RunMainCharacter" : "Idle");
    }
}
