using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_InactiveState : InactiveState
{
    private Enemy3 enemy;

    public E3_InactiveState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_InactiveState stateData, Transform rangedActiveStateData, Enemy3 enemy) : base(etity, stateMachine, animBoolName, stateData, rangedActiveStateData)
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

        if (isActive)
        {
            stateMachine.ChangeState(enemy.spawnState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
