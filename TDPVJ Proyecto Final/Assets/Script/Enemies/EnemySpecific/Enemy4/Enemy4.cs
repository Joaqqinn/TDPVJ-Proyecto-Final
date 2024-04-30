using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : Entity
{   
    public E4_MoveState moveState { get; private set; }
    public E4_IdleState idleState { get; private set; }
    public E4_PlayerDetectedState playerDetectedState { get; private set; }
    public E4_ChargeState chargeState { get; private set; }
    public E4_MeleeAttackState meleeAttackState { get; private set; }
    public E4_LookForPlayerState lookForPlayerState { get; private set; }
    //public E1_StunState stunState { get; private set; }
    public E4_DeadState deadState { get; private set; }
    public E4_DodgeState dodgeState { get; private set; }
    public E4_RangedAttackState rangedAttackState { get; private set; }

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
    //[SerializeField]
    //private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    public D_DodgeState dodgeStateData;
    [SerializeField]
    private D_RangedAttackState rangedAttackStateData;

    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform rangedAttackPosition;

    public override void Awake()
    {
        base.Awake();

        moveState = new E4_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E4_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E4_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        chargeState = new E4_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        meleeAttackState = new E4_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new E4_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        //stunState = new E2_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new E4_DeadState(this, stateMachine, "dead", deadStateData, this);
        dodgeState = new E4_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangedAttackState = new E4_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);

        //stats.Poise.OnCurrentValueZero += HandlePoiseZero;
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
