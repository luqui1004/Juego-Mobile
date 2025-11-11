using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArrowUI : MonoBehaviour
{
    public enum ArrowDirection { Up, Down, Left, Right }

    [Header("Arrow Config")]
    [SerializeField] private ArrowDirection arrowDirection = ArrowDirection.Up;
    public ArrowDirection Direction => arrowDirection;

    [Header("Movement")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private Vector2 direction = Vector2.right;

    [Header("LifeTime")]
    [SerializeField] private float lifetime = 3f;

    [Header("HitZone")]
    public RectTransform hitZone;
    private bool hasBeenSwiped = false;

    private RectTransform rectTransform;
    private float spawnTime;
    private System.Action<ArrowUI> onDestroyed;
    private bool destroyedByTimeout = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        spawnTime = Time.time;

        StartCoroutine(DestroyAfterLifetime());
    }

    void Update()
    {
        Move();
        CheckMissZone();
    }

    void Move()
    {
        rectTransform.anchoredPosition += direction * speed * Time.deltaTime;
    }

    void CheckMissZone()
    {
        if (hitZone == null) return;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, transform.position);
        bool isInside = RectTransformUtility.RectangleContainsScreenPoint(hitZone, screenPos);

        if (hasBeenSwiped || isInside) return;

        bool hasPassedZone = false;

        // Flechas horizontales
        if (direction.x > 0 && rectTransform.anchoredPosition.x > hitZone.anchoredPosition.x + hitZone.rect.width / 2f)
            hasPassedZone = true;
        else if (direction.x < 0 && rectTransform.anchoredPosition.x < hitZone.anchoredPosition.x - hitZone.rect.width / 2f)
            hasPassedZone = true;

        // Flechas verticales
        if (direction.y > 0 && rectTransform.anchoredPosition.y > hitZone.anchoredPosition.y + hitZone.rect.height / 2f)
            hasPassedZone = true;
        else if (direction.y < 0 && rectTransform.anchoredPosition.y < hitZone.anchoredPosition.y - hitZone.rect.height / 2f)
            hasPassedZone = true;

        if (hasPassedZone)
        {
            PlayerInputs playerInputs = FindObjectOfType<PlayerInputs>();
            if (playerInputs != null)
            {
                playerInputs.ShowFeedback("Bad");
            }

            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);

        if (!hasBeenSwiped)
        {
            destroyedByTimeout = true;
            PlayerInputs playerInputs = FindObjectOfType<PlayerInputs>();
            if (playerInputs != null)
            {
                playerInputs.ShowFeedback("Bad");
            }
        }

        Destroy(gameObject);
    }

    public void MarkAsSwiped()
    {
        hasBeenSwiped = true;
    }

    public void Initialize(System.Action<ArrowUI> onDestroyedCallback)
    {
        spawnTime = Time.time;
        onDestroyed = onDestroyedCallback;
    }

    void OnDestroy()
    {
        if (onDestroyed != null)
            onDestroyed(this);
    }

    public float GetElapsedTime()
    {
        return Time.time - spawnTime;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speed *= multiplier;
    }
}
