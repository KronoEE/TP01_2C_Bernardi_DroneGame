using UnityEngine.AI;
using UnityEngine;
using UnityHFSM;

namespace FSM
{
    public class CivilStateBase : StateBase<CivilState>
    {
        protected Civil Civil;
        protected NavMeshAgent Agent;
        protected Animator Animator;

        public CivilStateBase(bool needsExitTime, Civil civil) : base(needsExitTime)
        {
            Civil = civil;
            Agent = civil.GetAgent();
            Animator = civil.GetAnimator();
        }
    }
}