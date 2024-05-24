using System.Collections;
using System.Collections.Generic;
using Bardent.CoreSystem;
using UnityEngine;

public class FlyingState : State {

    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }

    private Movement movement;
    protected D_FlyingState stateData;

	protected GameObject routeMove;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;

    private int randomNum;
    private bool checkFlip;
    private Transform[] routePoints;
    public FlyingState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, GameObject routeMove, D_FlyingState stateData) : base(etity, stateMachine, animBoolName) {
        this.stateData = stateData;
		this.routeMove = routeMove;

        routePoints = routeMove.GetComponentsInChildren<Transform>();
		randomNum = 0;
    }

	public override void DoChecks() {
		base.DoChecks();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

	public override void Enter() {
		base.Enter();
    }

	public override void Exit() {
		base.Exit();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();


        if (checkFlip) {
			checkFlip = false;

            if ((Movement.FacingDirection == 1 && Movement.DefiningZeroAxis(routePoints[randomNum].position).x < 0))
            {
                Movement.Flip();
            } else if ((Movement.FacingDirection == -1 && Movement.DefiningZeroAxis(routePoints[randomNum].position).x >= 0)) 
            {
                Movement.Flip();
            }
        }

        Movement?.SetVelocity(stateData.movementSpeed, Movement.DefiningZeroAxis(routePoints[randomNum].position));

        if(Vector2.Distance(Movement.FindRelativePoint(Vector2.zero), routePoints[randomNum].position) < 3)
		{
			Debug.Log("LLEGASTE");
			checkFlip = true;
			randomNum = Random.Range(0, routePoints.Length);
		}

    }

    public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
