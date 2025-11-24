using System.Collections;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [Header("Configuración del Spawn")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform enemyParent;

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            return;
        }

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, enemyParent);
        EnemyFinal enemyFinal = newEnemy.GetComponent<EnemyFinal>();
        if (enemyFinal != null)
            StartCoroutine(ReiniciarDespuesDeUnFrame(enemyFinal));
    }

    private IEnumerator ReiniciarDespuesDeUnFrame(EnemyFinal enemy)
    {
        yield return null;
        enemy.ReiniciarReferencias();
    }
}
