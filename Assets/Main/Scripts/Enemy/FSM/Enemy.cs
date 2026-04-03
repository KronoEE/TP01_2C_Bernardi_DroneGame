using UnityHFSM;
using Sensors;
using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private PlayerController Player;

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

        [Space]
        [Header("Debug Info")]
        [SerializeField]
        private bool IsInChasingRange;
        [SerializeField]
        private float LastAttackTime;

        [Header("Range Attack")]
        [SerializeField] private Transform WandTip;
        [SerializeField] private GameObject ProjectilePrefab;

        private StateMachine<EnemyState, StateEvent> EnemyFSM;
        private Animator Animator;
        private NavMeshAgent Agent;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            EnemyFSM = new();

            // Add States
            EnemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
            EnemyFSM.AddState(EnemyState.Chase, new ChaseState(true, this, Player.transform));
            EnemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack));

            // Add Transitions
            EnemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));
            EnemyFSM.AddTriggerTransition(StateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
                (transition) => IsInChasingRange
                                && Vector3.Distance(Player.transform.position, transform.position) > Agent.stoppingDistance)
            );

            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
                (transition) => !IsInChasingRange
                                || IsInAttackRange)
            );

            // Attack Transitions
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldAttack, forceInstantly: true));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldAttack, forceInstantly: true));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, IsNotWithinIdleRange));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));

            EnemyFSM.Init();
        }

        private void Start()
        {
            FollowPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerEnter;
            FollowPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerExit;
            RangeAttackPlayerSensor.OnPlayerEnter += RangeAttackPlayerSensor_OnPlayerEnter;
            RangeAttackPlayerSensor.OnPlayerExit += RangeAttackPlayerSensor_OnPlayerExit;
        }

        private void FollowPlayerSensor_OnPlayerExit(Vector3 LastKnownPosition)
        {
            EnemyFSM.Trigger(StateEvent.LostPlayer);
            IsInChasingRange = false;
        }

        private void FollowPlayerSensor_OnPlayerEnter(Transform Player)
        {
            EnemyFSM.Trigger(StateEvent.DetectPlayer);
            IsInChasingRange = true;
        }

        private bool ShouldAttack(Transition<EnemyState> Transition) =>
            LastAttackTime + AttackCooldown <= Time.time
            && IsInAttackRange;

        private bool IsWithinIdleRange(Transition<EnemyState> Transition) =>
            Vector3.Distance(Player.transform.position, transform.position) <= Agent.stoppingDistance;

        private bool IsNotWithinIdleRange(Transition<EnemyState> Transition) =>
            !IsWithinIdleRange(Transition);

        private void RangeAttackPlayerSensor_OnPlayerEnter(Transform Player) => IsInAttackRange = true;
        private void RangeAttackPlayerSensor_OnPlayerExit(Vector3 LastKnownPosition) => IsInAttackRange = false;

        private void OnAttack(State<EnemyState, StateEvent> State)
        {
            transform.LookAt(Player.transform.position);
            LastAttackTime = Time.time;

            GameObject obj = Instantiate(ProjectilePrefab, WandTip.position, WandTip.rotation);
            obj.SetActive(true);
        }

        private void Update()
        {
            EnemyFSM.OnLogic();
        }
    }
}