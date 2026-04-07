using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Config")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemiesInScene = 5;

    private float spawnTimer;

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnEnemy();
        }
    }
    private void TrySpawnEnemy()
    {
        if (PoolManager.Instance.GetActiveEnemyCount() >= maxEnemiesInScene)
            return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        FSM.Enemy enemy = PoolManager.Instance.GetEnemy();

        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;
        }
    }
}
