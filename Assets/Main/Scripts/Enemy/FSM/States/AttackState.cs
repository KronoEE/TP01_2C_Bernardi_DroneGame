using FSM;
using System;
using UnityEngine;
using UnityHFSM;

public class AttackState : EnemyStateBase
{
    private readonly Action<State<EnemyState, StateEvent>> onAttack;

    public AttackState(
        bool needsExitTime,
        Enemy enemy,
        Action<State<EnemyState, StateEvent>> onAttack,
        float exitTime = 0.33f) : base(needsExitTime, enemy, exitTime, onEnter: onAttack)
    {
        this.onAttack = onAttack;
    }

    public override void OnEnter()
    {
        Agent.isStopped = true;
        Agent.velocity = Vector3.zero;
        base.OnEnter();
        Animator.CrossFade("Attack", 0.1f);
    }

    public override void OnLogic()
    {
        // Rotar hacia el player
        if (Enemy.Player != null)
        {
            Vector3 direction = Enemy.Player.transform.position - Enemy.transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
                Enemy.transform.rotation = Quaternion.LookRotation(direction);
        }

        var currentAnim = Animator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnim.IsName("Attack"))
            Animator.CrossFade("Idle_A", 0.15f);

        base.OnLogic();
    }

    public override void OnExit()
    {
        base.OnExit();
        Agent.isStopped = false;
    }
}