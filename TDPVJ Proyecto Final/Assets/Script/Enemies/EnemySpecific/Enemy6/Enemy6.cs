using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6 : Entity
{   
    public E6_MoveState moveState { get; private set; }
    public E6_IdleState idleState { get; private set; }
    public E6_PlayerDetectedState playerDetectedState { get; private set; }
    public E6_ChargeState chargeState { get; private set; }
    public E6_MeleeAttackState meleeAttackState { get; private set; }
    public E6_LookForPlayerState lookForPlayerState { get; private set; }
    public E6_DeadState deadState { get; private set; }
    public E6_DodgeState dodgeState { get; private set; }

    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    public D_DodgeState dodgeStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        moveState = new E6_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E6_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E6_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        chargeState = new E6_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        meleeAttackState = new E6_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new E6_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        deadState = new E6_DeadState(this, stateMachine, "dead", deadStateData, this);
        dodgeState = new E6_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
    }

    /*private void HandlePoiseZero()
    {
        stateMachine.ChangeState(stunState);
    }

    private void OnDestroy()
    {
        stats.Poise.OnCurrentValueZero -= HandlePoiseZero;
    }*/

    protected override void HandleParry()
    {
        base.HandleParry();

        Debug.Log("PARRY STUN STATE");
        //stateMachine.ChangeState(stunState);
    }

    private void Start()
    {
        stateMachine.Initialize(moveState);        
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
