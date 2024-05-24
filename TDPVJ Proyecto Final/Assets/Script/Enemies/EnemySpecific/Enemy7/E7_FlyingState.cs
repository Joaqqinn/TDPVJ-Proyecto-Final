using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E7_FlyingState : FlyingState
{
    private Enemy7 enemy;
    public E7_FlyingState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, GameObject routeMove, D_FlyingState stateData, Enemy7 enemy) : base(etity, stateMachine, animBoolName, routeMove, stateData)
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

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
