using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace FSM
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public class Civil : MonoBehaviour, IDamageable
    {
        [Header("Wander Config")]
        [SerializeField] private float wanderRadius = 20f;
        [SerializeField] private float waitDuration = 2f;

        private StateMachine<CivilState> CivilFSM;
        private Animator Animator;
        private NavMeshAgent Agent;
        private HealthSystem healthSystem;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            healthSystem = GetComponent<HealthSystem>();

            CivilFSM = new();

            CivilFSM.AddState(CivilState.Idle, new CivilIdleState(false, this));
            CivilFSM.AddState(CivilState.Walk, new CivilWalkState(false, this, wanderRadius, waitDuration));

            // Idle → Walk
            CivilFSM.AddTransition(new Transition<CivilState>(CivilState.Idle, CivilState.Walk,
                t => true));

            CivilFSM.Init();
        }
        private void Update()
        {
            CivilFSM.OnLogic();
        }
        public void TakeDamage(int damage)
        {
            healthSystem.TakeDamage(damage);
        }
        public NavMeshAgent GetAgent() => Agent;
        public Animator GetAnimator() => Animator;
    }
}