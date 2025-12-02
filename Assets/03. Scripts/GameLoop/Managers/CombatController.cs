using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public static CombatController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Header("UI Combat")]
    public GameObject combatUI;

    [Header("Arrow Settings")]
    public List<GameObject> arrowPrefabs;
    public RectTransform spawnPoint;
    public RectTransform despawnPoint;
    public RectTransform targetZone;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;

    private Coroutine spawnRoutine;

    public Enemy currentEnemy;

    public List<ArrowUI> activeArrows = new List<ArrowUI>();

    private Animator anim;

    public void StartCombat()
    {
        CameraManager.Instance.DoZoomIn();
        ParallaxManager.Instance.StopParallax();
        ClearAllArrows();

        combatUI.SetActive(true);
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void EndCombat()
    {
        CameraManager.Instance.DoZoomOut();
        ParallaxManager.Instance.RenaudeParallax();
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        ClearAllArrows();

        currentEnemy = null;

        combatUI.SetActive(false);
    }

    private void ClearAllArrows()
    {
        foreach (var a in activeArrows)
            if (a != null) Destroy(a.gameObject);

        activeArrows.Clear();
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnRandomArrow();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRandomArrow()
    {
        if (arrowPrefabs.Count == 0) return;

        int index = Random.Range(0, arrowPrefabs.Count);

        GameObject arrowGO = Instantiate(arrowPrefabs[index], spawnPoint.parent);
        RectTransform rt = arrowGO.GetComponent<RectTransform>();

        rt.anchoredPosition = spawnPoint.anchoredPosition;

        ArrowUI arrow = arrowGO.AddComponent<ArrowUI>();
        arrow.despawnPoint = despawnPoint;
        arrow.targetZone = targetZone;

        activeArrows.Add(arrow);
    }

    public void SetCombatStats(float newSpeed, float newInterval)
    {
        ArrowUI.arrowSpeed = newSpeed;
        spawnInterval = newInterval;
    }

    public ArrowUI GetClosestArrowInTarget(ArrowSet.ArrowType type)
    {
        ArrowUI best = null;
        float bestDist = float.MaxValue;
        float targetX = targetZone.anchoredPosition.x;

        foreach (var a in activeArrows)
        {
            if (a == null) continue;
            if (!a.IsInsideTarget()) continue;
            if (a.arrowSet.arrowType != type) continue;

            float dist = Mathf.Abs(a.GetRectTransform().anchoredPosition.x - targetX);
            if (dist < bestDist)
            {
                bestDist = dist;
                best = a;
            }
        }

        return best;
    }

    // Variante que devuelve la flecha dentro del target (de cualquier tipo) más cercana al centro.
    public ArrowUI GetClosestArrowInTargetAny()
    {
        ArrowUI best = null;
        float bestDist = float.MaxValue;
        float targetX = targetZone.anchoredPosition.x;

        foreach (var a in activeArrows)
        {
            if (a == null) continue;
            if (!a.IsInsideTarget()) continue;

            float dist = Mathf.Abs(a.GetRectTransform().anchoredPosition.x - targetX);
            if (dist < bestDist)
            {
                bestDist = dist;
                best = a;
            }
        }

        return best;
    }

    public void MissByWrongSwipe()
    {
        if (activeArrows.Count == 0) return;

        ArrowUI closest = GetClosestArrowInTargetAny();

        if (closest == null)
        {
            float closestDistance = float.MaxValue;
            foreach (var a in activeArrows)
            {
                if (a == null) continue;

                float distance = Mathf.Abs(a.GetRectTransform().anchoredPosition.x - targetZone.anchoredPosition.x);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = a;
                }
            }
        }

        TextSpawnManager.Instance.SpawnBadText();
        CameraManager.Instance.DoShake(0.1f, 0.5f);

        if (closest != null)
        {
            closest.Miss();
            closest.DestroyArrow();
        }
    }

    public class ArrowUI : MonoBehaviour
    {
        public static float arrowSpeed = 2f;

        public RectTransform despawnPoint;
        public RectTransform targetZone;

        private RectTransform rt;
        public ArrowSet arrowSet;
        private bool insideTarget = false;

        private float prevX;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            arrowSet = GetComponent<ArrowSet>();
            prevX = rt.anchoredPosition.x;
        }

        private void Update()
        {
            float delta = arrowSpeed * Time.deltaTime;
            float newX = rt.anchoredPosition.x + delta;

            rt.anchoredPosition = new Vector2(newX, rt.anchoredPosition.y);

            if (rt.anchoredPosition.x > despawnPoint.anchoredPosition.x)
            {
                Miss();
                DestroyArrow();
                return;
            }

            if (CrossedTarget(prevX, newX))
            {
                if (!insideTarget)
                {
                    insideTarget = true;
                }
            }
            else
            {
                if (insideTarget)
                {
                    insideTarget = false;
                }
            }

            prevX = rt.anchoredPosition.x;
        }

        private bool CrossedTarget(float previousX, float currentX)
        {
            float targetCenter = targetZone.anchoredPosition.x;
            float halfWidth = targetZone.rect.width / 2f;
            float left = targetCenter - halfWidth;
            float right = targetCenter + halfWidth;

            if ((previousX >= left && previousX <= right) || (currentX >= left && currentX <= right))
                return true;

            if ((previousX < left && currentX > right) || (previousX < left && currentX >= left) || (previousX <= right && currentX > right))
                return true;

            return false;
        }

        public bool IsInsideTarget()
        {
            return insideTarget;
        }

        public RectTransform GetRectTransform() => rt;

        public void DestroyArrow()
        {
            if (CombatController.Instance != null)
                CombatController.Instance.activeArrows.Remove(this);

            Destroy(gameObject);
        }

        public void Miss()
        {
            if (CombatController.Instance != null && CombatController.Instance.currentEnemy != null)
                ScoreManager.Instance.TakeDamage(CombatController.Instance.currentEnemy.Damage);

            CombatController.Instance.currentEnemy.anim.SetTrigger("Attack");
            TextSpawnManager.Instance.SpawnBadText();
            CameraManager.Instance.DoShake(0.1f, 0.5f);
        }
    }
}
