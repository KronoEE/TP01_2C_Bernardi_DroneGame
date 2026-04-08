namespace FSM
{
    public class CivilIdleState : CivilStateBase
    {
        public CivilIdleState(bool needsExitTime, Civil civil)
            : base(needsExitTime, civil) { }

        public override void OnEnter()
        {
            Agent.isStopped = true;
            Animator.Play("Idle");
        }
        public override void OnExit()
        {
            Agent.isStopped = false;
        }
    }
}