using UnityEngine;

namespace FSM
{
    public class ChaseState : EnemyStateBase
    {
        private Transform Target;

        public ChaseState(bool needsExitTime, Enemy Enemy, Transform Target) : base(needsExitTime, Enemy)
        {
            this.Target = Target;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.enabled = true;
            Agent.isStopped = false;
            Agent.speed = 5f;
            Animator.Play("Run");
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (!RequestedExit)
            {
                Agent.SetDestination(Target.position);

                // Si llegó al stopping distance, que quede mirando al jugador
                if (Agent.remainingDistance <= Agent.stoppingDistance)
                {
                    Enemy.transform.LookAt(Target);
                }
            }
            else if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                fsm.StateCanExit();
            }
        }
    }
}