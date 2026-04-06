using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    public class PatrolState : EnemyStateBase
    {
        private readonly float wanderRadius;
        private readonly float waitDuration;
        private float waitTimer;
        private bool isWaiting;

        public PatrolState(bool needsExitTime, Enemy enemy, float wanderRadius = 20f, float waitDuration = 2f)
            : base(needsExitTime, enemy)
        {
            this.wanderRadius = wanderRadius;
            this.waitDuration = waitDuration;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.isStopped = false;
            Agent.speed = 2f;
            Animator.Play("Walk");
            GoToRandomPoint();
        }

        public override void OnLogic()
        {
            base.OnLogic();

            if (Agent.pathPending) return;

            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!isWaiting)
                {
                    isWaiting = true;
                    waitTimer = waitDuration;
                    if (HasAnimation("Idle")) Animator.Play("Idle");
                }

                waitTimer -= Time.deltaTime;

                if (waitTimer <= 0f)
                {
                    isWaiting = false;
                    if (HasAnimation("Walk")) Animator.Play("Walk");
                    GoToRandomPoint();
                }
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            isWaiting = false;
            Agent.ResetPath();
        }
        private void GoToRandomPoint()
        {
            Vector3 destination;
            if (TryGetRandomNavMeshPoint(Enemy.transform.position, wanderRadius, out destination))
                Agent.SetDestination(destination);
        }
        private bool TryGetRandomNavMeshPoint(Vector3 center, float radius, out Vector3 result)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 randomPoint = center + Random.insideUnitSphere * radius;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = center;
            return false;
        }
        private bool HasAnimation(string stateName)
        {
            return Animator.HasState(0, Animator.StringToHash(stateName));
        }
    }
}