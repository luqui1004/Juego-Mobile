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

    public ArrowSet.ArrowType? currentArrowInTarget = null;
    public ArrowUI currentArrowUI;
    public Enemy currentEnemy;

    public void StartCombat()
    {
        ClearAllArrows();

        currentArrowUI = null;
        currentArrowInTarget = null;

        combatUI.SetActive(true);
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void EndCombat()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        ClearAllArrows();
        currentArrowInTarget = null;
        currentArrowUI = null;

        currentEnemy = null;

        combatUI.SetActive(false);
    }

    private void ClearAllArrows()
    {
        ArrowUI[] arrows = FindObjectsOfType<ArrowUI>();

        foreach (var a in arrows)
            Destroy(a.gameObject);
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

        currentArrowUI = arrow;
    }

    public void SetCombatStats(float newSpeed, float newInterval)
    {
        ArrowUI.arrowSpeed = newSpeed;
        spawnInterval = newInterval;
    }


    // -------- ARROW UI ----------
    public class ArrowUI : MonoBehaviour
    {
        public static float arrowSpeed = 2f;

        public RectTransform despawnPoint;
        public RectTransform targetZone;

        private RectTransform rt;
        private ArrowSet arrowSet;
        private bool insideTarget = false;
        public bool processed = false;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            arrowSet = GetComponent<ArrowSet>();
        }

        private void Update()
        {
            rt.anchoredPosition += Vector2.right * arrowSpeed * Time.deltaTime;

            if (rt.anchoredPosition.x > despawnPoint.anchoredPosition.x)
            {
                Miss();
                Destroy(gameObject);
                return;
            }

            if (IsInsideTarget())
            {
                if (!insideTarget)
                {
                    insideTarget = true;
                    CombatController.Instance.currentArrowInTarget = arrowSet.arrowType;
                    CombatController.Instance.currentArrowUI = this;
                }
            }
            else
            {
                if (insideTarget)
                {
                    insideTarget = false;
                }
            }
        }

        private bool IsInsideTarget()
        {
            float halfWidth = targetZone.rect.width / 2f;
            return Mathf.Abs(rt.anchoredPosition.x - targetZone.anchoredPosition.x) < halfWidth;
        }

        public void DestroyArrow()
        {
            CombatController.Instance.currentArrowInTarget = null;
            CombatController.Instance.currentArrowUI = null;

            Destroy(gameObject);
        }

        private void Miss()
        {
            CombatController.Instance.currentArrowInTarget = null;
            CombatController.Instance.currentArrowUI = null;

            if (CombatController.Instance.currentEnemy != null)
                ScoreManager.Instance.TakeDamage(CombatController.Instance.currentEnemy.Damage);
        }
    }
}
