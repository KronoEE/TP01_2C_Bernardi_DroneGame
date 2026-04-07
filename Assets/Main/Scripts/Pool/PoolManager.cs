using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private EnemyBullet enemyBulletPrefab;
    [SerializeField] private PlayerRocket playerRocketPrefab;
    [SerializeField] private FSM.Enemy enemyPrefab;
    [SerializeField] private FSM.Civil civilPrefab;

    [Header("Capacidades")]
    [SerializeField] private int maxEnemyBullets = 20;
    [SerializeField] private int maxPlayerRockets = 10;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private int maxCivilians = 15;

    [Header("Containers")]
    private Transform enemyBulletContainer;
    private Transform playerRocketContainer;
    private Transform enemyContainer;
    private Transform civilContainer;

    // Pools
    private ObjectPool<EnemyBullet> enemyBulletPool;
    private ObjectPool<PlayerRocket> playerRocketPool;
    private ObjectPool<FSM.Enemy> enemyPool;
    private ObjectPool<FSM.Civil> civilPool;

    public int GetActiveEnemyCount() => enemyPool.ActiveCount();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateContainers();
        InitializePools();
    }

    private void CreateContainers()
    {
        enemyBulletContainer = CreateContainer("EnemyBullets");
        playerRocketContainer = CreateContainer("PlayerRockets");
        enemyContainer = CreateContainer("Enemies");
        civilContainer = CreateContainer("Civilians");
    }

    private Transform CreateContainer(string name)
    {
        GameObject container = new GameObject(name);
        container.transform.SetParent(transform);
        return container.transform;
    }

    private void InitializePools()
    {
        enemyBulletPool = new ObjectPool<EnemyBullet>(enemyBulletPrefab, enemyBulletContainer, 5, maxEnemyBullets);
        playerRocketPool = new ObjectPool<PlayerRocket>(playerRocketPrefab, playerRocketContainer, 3, maxPlayerRockets);
        enemyPool = new ObjectPool<FSM.Enemy>(enemyPrefab, enemyContainer, 3, maxEnemies);
        civilPool = new ObjectPool<FSM.Civil>(civilPrefab, civilContainer, 5, maxCivilians);
    }

    // Getters
    public EnemyBullet GetEnemyBullet() => enemyBulletPool.Get();
    public PlayerRocket GetPlayerRocket() => playerRocketPool.Get();
    public FSM.Enemy GetEnemy() => enemyPool.Get();
    public FSM.Civil GetCivil() => civilPool.Get();

    public void ReturnEnemyBullet(EnemyBullet obj) => enemyBulletPool.Return(obj);
    public void ReturnPlayerRocket(PlayerRocket obj) => playerRocketPool.Return(obj);
    public void ReturnEnemy(FSM.Enemy obj) => enemyPool.Return(obj);
    public void ReturnCivil(FSM.Civil obj) => civilPool.Return(obj);
}