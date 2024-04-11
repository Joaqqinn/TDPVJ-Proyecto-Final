using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_SpawnState : SpawnState
{
    private Enemy3 enemy;

    public E3_SpawnState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_SpawnState stateData, Enemy3 enemy) : base(etity, stateMachine, animBoolName, stateData)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void AnimationFinished()
    {
        base.AnimationFinished();
    }
}
