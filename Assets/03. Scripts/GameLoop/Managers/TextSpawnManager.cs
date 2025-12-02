using UnityEngine;
public class TextSpawnManager : MonoBehaviour
{
    public static TextSpawnManager Instance { get; private set; }

    [Header("Prefabs Texts")]
    public GameObject badTextPrefab;
    public GameObject perfectTextPrefab;

    [Header("UI SpawnPoint")]
    public Transform spawnPoint;

    [Header("Destroy Text")]
    public float destroyTime = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnBadText()
    {
        Spawn(badTextPrefab);
    }

    public void SpawnPerfectText()
    {
        Spawn(perfectTextPrefab);
    }

    private void Spawn(GameObject prefab)
    {
        if (prefab == null)
        {
            return;
        }

        if (spawnPoint == null)
        {
            return;
        }

        GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        Destroy(obj, destroyTime);
    }
}
