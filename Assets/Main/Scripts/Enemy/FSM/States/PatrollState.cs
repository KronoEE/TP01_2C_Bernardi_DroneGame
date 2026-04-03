using UnityEngine;

namespace FSM
{
    public class PatrolState : EnemyStateBase
    {
        private Transform[] Waypoints;
        private int CurrentWaypoint = 0;

        public PatrolState(bool needsExitTime, Enemy Enemy, Transform[] Waypoints)
            : base(needsExitTime, Enemy)
        {
            this.Waypoints = Waypoints;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.isStopped = false;
            Agent.speed = 2f;
            Animator.Play("Walk");
            GoToNextWaypoint();
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                GoToNextWaypoint();
            }
        }

        private void GoToNextWaypoint()
        {
            if (Waypoints.Length == 0) return;
            Agent.SetDestination(Waypoints[CurrentWaypoint].position);
            CurrentWaypoint = (CurrentWaypoint + 1) % Waypoints.Length;
        }
    }
}