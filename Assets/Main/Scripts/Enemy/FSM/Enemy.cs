using UnityHFSM;
using Sensors;
using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private HealthSystem healthSystem;

        [Header("Score")]
        [SerializeField] private int scoreValue = 100;

        // Debug Info
        [Space]
        [Header("Debug Info")]
        [SerializeField] private bool IsInChasingRange;
        [SerializeField] private float LastAttackTime;

        [Header("Attack Config")]
        [SerializeField]
        [Range(0.1f, 5f)]
        private float AttackCooldown = 2;
        private bool IsInAttackRange;

        [Header("Sensors")]
        [SerializeField]
        private PlayerSensor FollowPlayerSensor;
        [SerializeField]
        private PlayerSensor RangeAttackPlayerSensor;

        [Header("Range Attack")]
        [SerializeField] private Transform WandTip;
        [SerializeField] private GameObject ProjectilePrefab;

        public PlayerController Player { get; private set; }
        private StateMachine<EnemyState, StateEvent> EnemyFSM;
        private Animator Animator;
        private NavMeshAgent Agent;
        AudioManager audioManager;
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }
        private void Start()
        {
            FollowPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerEnter;
            FollowPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerExit;
            RangeAttackPlayerSensor.OnPlayerEnter += RangeAttackPlayerSensor_OnPlayerEnter;
            RangeAttackPlayerSensor.OnPlayerExit += RangeAttackPlayerSensor_OnPlayerExit;

            healthSystem.onDie += OnDie;

            Player = PlayerController.Instance;

            if (Player == null)
            {
                Debug.LogError($"{gameObject.name}: PlayerController.Instance es null");
                return;
            }

            InitializeFSM();
        }
        public void InitializeFSM()
        {
            EnemyFSM = new();

            // States
            EnemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
            EnemyFSM.AddState(EnemyState.Patrol, new PatrolState(false, this, 20f, 2f));
            EnemyFSM.AddState(EnemyState.Chase, new ChaseState(true, this, Player.transform));
            EnemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack, AttackCooldown));
            EnemyFSM.AddState(EnemyState.Die, new IdleState(false, this));

            // Transitions
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Patrol, t => true));

            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Chase, t => IsInChasingRange));
            EnemyFSM.AddTriggerTransition(StateEvent.DetectPlayer,
                new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Chase));

            EnemyFSM.AddTriggerTransition(StateEvent.DetectPlayer,
                new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));

            EnemyFSM.AddTriggerTransition(StateEvent.LostPlayer,
                new Transition<EnemyState>(EnemyState.Chase, EnemyState.Patrol));

            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
                t => IsInChasingRange &&
                Vector3.Distance(Player.transform.position, transform.position) > Agent.stoppingDistance));

            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldAttack));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldAttack));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, IsNotWithinIdleRange));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Patrol, IsWithinIdleRange));

            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
                t => !IsInChasingRange));

            EnemyFSM.Init();
        }
        private void FollowPlayerSensor_OnPlayerExit(Vector3 LastKnownPosition)
        {
            if (EnemyFSM == null) return;

            EnemyFSM.Trigger(StateEvent.LostPlayer);
            IsInChasingRange = false;
        }
        private void FollowPlayerSensor_OnPlayerEnter(Transform Player)
        {
            if (EnemyFSM == null) return;

            EnemyFSM.Trigger(StateEvent.DetectPlayer);
            IsInChasingRange = true;
        }
        private bool ShouldAttack(Transition<EnemyState> Transition)
        {
            bool cooldownOk = LastAttackTime + AttackCooldown <= Time.time;
            bool inRange = IsInAttackRange;

            return cooldownOk && inRange;
        }
        private bool IsWithinIdleRange(Transition<EnemyState> Transition)
        {
            if (Player == null) return true;
            return Vector3.Distance(Player.transform.position, transform.position) <= Agent.stoppingDistance;
        }

        private bool IsNotWithinIdleRange(Transition<EnemyState> Transition)
        {
            if (Player == null) return false;
            return !IsWithinIdleRange(Transition);
        }
        private void RangeAttackPlayerSensor_OnPlayerEnter(Transform Player) => IsInAttackRange = true;
        private void RangeAttackPlayerSensor_OnPlayerExit(Vector3 LastKnownPosition) => IsInAttackRange = false;
        private void OnAttack(State<EnemyState, StateEvent> State)
        {
            if (Player == null) return;

            LastAttackTime = Time.time;
            audioManager.PlaySFX(audioManager.EnemyAttackSfx);
            Vector3 targetPosition = Player.transform.position + Vector3.up * 0.5f;
            transform.LookAt(targetPosition);

            EnemyBullet bullet = PoolManager.Instance.GetEnemyBullet();

            if (bullet == null) return;

            bullet.transform.position = WandTip.position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.Initialize(targetPosition);
        }
        private void Update()
        {
            if (EnemyFSM == null) return;
            if (Player == null) return;
            EnemyFSM.OnLogic();
        }
        private void OnEnable()
        {
            if (healthSystem != null)
                healthSystem.ResetHealth();

            IsInChasingRange = false;
            LastAttackTime = 0f;

            Player = PlayerController.Instance;

            if (EnemyFSM != null)
            {
                if (Player != null)
                {
                    InitializeFSM();
                }
                else
                {
                    EnemyFSM.RequestStateChange(EnemyState.Patrol);
                }
            }        
        }
        private void OnDie()
        {
            audioManager.PlaySFX(audioManager.EnemyDeathSfx);
            ScoreSystem.Instance?.AddScore(scoreValue);
            LevelManager.Instance?.OnEnemyKilled();
            PoolManager.Instance?.ReturnEnemy(this);
        }
    }
}