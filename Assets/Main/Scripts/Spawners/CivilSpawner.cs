using UnityEngine;

public class CivilSpawner : MonoBehaviour
{
    [Header("Spawn Config")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 8f;
    [SerializeField] private int maxCiviliansInScene = 5;

    private float spawnTimer;

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnCivil();
        }
    }

    private void TrySpawnCivil()
    {
        if (PoolManager.Instance.GetActiveCivilCount() >= maxCiviliansInScene) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        FSM.Civil civil = PoolManager.Instance.GetCivil();

        if (civil != null)
        {
            civil.transform.position = spawnPoint.position;
            civil.transform.rotation = spawnPoint.rotation;
        }
    }
}