using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowSpawner : MonoBehaviour
{
    [Header("Spawn Config")]
    [SerializeField] private GameObject[] arrowPrefabs;
    [SerializeField] private Transform spawnPoint;

    [Header("HitZone")]
    [SerializeField] private RectTransform hitZone;

    [Header("Difficulty")]
    [SerializeField] private float arrowSpeedMultiplier = 1f;
    [SerializeField] private float spawnInterval = 1.8f;

    private Coroutine spawnRoutine;
    private List<ArrowUI> activeArrows = new List<ArrowUI>();

    void Start()
    {
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (CombatManager.Instance != null && CombatManager.Instance.isInCombat)
                SpawnArrow();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnArrow()
    {
        if (arrowPrefabs == null || arrowPrefabs.Length == 0 || spawnPoint == null)
        {
            return;
        }

        GameObject prefab = arrowPrefabs[Random.Range(0, arrowPrefabs.Length)];
        GameObject newArrow = Instantiate(prefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);

        RectTransform arrowRect = newArrow.GetComponent<RectTransform>();
        RectTransform spawnRect = spawnPoint.GetComponent<RectTransform>();
        if (arrowRect != null && spawnRect != null)
        {
            arrowRect.anchoredPosition = spawnRect.anchoredPosition;
        }

        ArrowUI arrowScript = newArrow.GetComponent<ArrowUI>();
        if (arrowScript != null)
        {
            arrowScript.hitZone = hitZone;

            arrowScript.SetSpeedMultiplier(arrowSpeedMultiplier);

            activeArrows.Add(arrowScript);

            arrowScript.Initialize(RemoveArrowFromList);
        }
    }

    private void RemoveArrowFromList(ArrowUI arrow)
    {
        if (activeArrows.Contains(arrow))
            activeArrows.Remove(arrow);
    }

    public ArrowUI GetClosestArrow()
    {
        return activeArrows.Count > 0 ? activeArrows[0] : null;
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    public void SetDifficulty(float multiplier)
    {
        arrowSpeedMultiplier = Mathf.Max(0.1f, multiplier);
    }
}
