using System.Collections;
using System.Collections.Generic;
using Bardent.CoreSystem;
using UnityEngine;

public class SpawnState : State {

    protected D_SpawnState stateData;

	protected bool isAnimationFinished;

    public SpawnState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_SpawnState stateData) : base(etity, stateMachine, animBoolName) {
		this.stateData = stateData;
	}

	public override void DoChecks() {
		base.DoChecks();
	}

	public override void Enter() {
		base.Enter();

        entity.atsm.spawnState = this;
        isAnimationFinished = false;
    }

	public override void Exit() {
		base.Exit();
	}

    public override void PhysicsUpdate() {
		base.PhysicsUpdate();
    }

	public virtual void AnimationFinished()
	{
		isAnimationFinished = true;
		Debug.Log("FINIS ANIM");
	}
}
