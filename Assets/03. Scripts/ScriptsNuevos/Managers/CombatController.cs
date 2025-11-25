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

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;

    private Coroutine spawnRoutine;

    public void StartCombat()
    {
        ClearAllArrows();

        combatUI.SetActive(true);
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void EndCombat()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        ClearAllArrows();

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

        ArrowUI despawn = arrowGO.AddComponent<ArrowUI>();
        despawn.despawnPoint = despawnPoint;
    }

    public void SetCombatStats(float newSpeed, float newInterval)
    {
        // Cambiar velocidad de flecha
        ArrowUI.arrowSpeed = newSpeed;

        // Cambiar intervalo de spawn
        spawnInterval = newInterval;
    }


    // CLASE INTERNA SOLO PARA EL DESPAWN
    private class ArrowUI : MonoBehaviour
    {
        public static float arrowSpeed = 2f;

        public RectTransform despawnPoint;
        private RectTransform rt;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rt.anchoredPosition += Vector2.right * arrowSpeed * Time.deltaTime;

            if (rt.anchoredPosition.x > despawnPoint.anchoredPosition.x)
                Destroy(gameObject);
        }
    }
}
