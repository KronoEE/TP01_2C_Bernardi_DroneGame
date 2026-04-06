using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    public class CivilWalkState : CivilStateBase
    {
        private float wanderRadius;
        private float waitDuration;
        private float waitTimer;
        private bool isWaiting;

        public CivilWalkState(bool needsExitTime, Civil civil, float wanderRadius, float waitDuration)
            : base(needsExitTime, civil)
        {
            this.wanderRadius = wanderRadius;
            this.waitDuration = waitDuration;
        }
        public override void OnEnter()
        {
            Agent.isStopped = false;
            Agent.speed = 1.5f;
            Animator.Play("Walk");
            GoToRandomPoint();
        }
        public override void OnLogic()
        {
            if (Agent.pathPending) return;

            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!isWaiting)
                {
                    isWaiting = true;
                    waitTimer = waitDuration;
                    Animator.Play("Idle");
                }

                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    isWaiting = false;
                    Animator.Play("Walk");
                    GoToRandomPoint();
                }
            }
        }
        public override void OnExit()
        {
            isWaiting = false;
            Agent.ResetPath();
        }
        private void GoToRandomPoint()
        {
            Vector3 randomPoint = Civil.transform.position + Random.insideUnitSphere * wanderRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, wanderRadius, NavMesh.AllAreas))
                Agent.SetDestination(hit.position);
        }
    }
}