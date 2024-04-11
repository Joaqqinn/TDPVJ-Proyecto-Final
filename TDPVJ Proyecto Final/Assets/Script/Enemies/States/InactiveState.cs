using System.Collections;
using System.Collections.Generic;
using Bardent.CoreSystem;
using UnityEngine;

public class InactiveState : State {

    protected D_InactiveState stateData;

    protected Transform rangedActiveStateData;

	protected bool isActive;

    public InactiveState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_InactiveState stateData, Transform rangedActiveStateData) : base(etity, stateMachine, animBoolName) {
		this.stateData = stateData;
		this.rangedActiveStateData = rangedActiveStateData;
	}

	public override void DoChecks() {
		base.DoChecks();
	}

	public override void Enter() {
		base.Enter();
		isActive = false;
    }

	public override void Exit() {
		base.Exit();
	}

    public override void PhysicsUpdate() {
		base.PhysicsUpdate();

        Collider2D detectedObjects = Physics2D.OverlapCircle(rangedActiveStateData.position, stateData.activeRadius, stateData.whatIsPlayer);

		//Cuando ingresa al radio activamos el personaje
		if(detectedObjects)
		{
			isActive = true;
        }
    }
}
