using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E6_DodgeState : DodgeState
{
    private Enemy6 enemy;

    public E6_DodgeState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData, Enemy6 enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDodgeOver)
        {
            if (isPlayerInMaxAgroRange && performCloseRangeAction)
            {
                Debug.Log("MELEE ATTACK1");
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
            /*else if (isPlayerInMaxAgroRange && !performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.rangedAttackState);
            }*/
            else if (!isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }

            //TODO: ranged attack state
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
