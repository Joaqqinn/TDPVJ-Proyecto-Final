using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Entity
{   
    public E5_MoveState moveState { get; private set; }
    public E5_IdleState idleState { get; private set; }
    public E5_PlayerDetectedState playerDetectedState { get; private set; }
    public E5_MeleeAttackState meleeAttackState { get; private set; }
    public E5_LookForPlayerState lookForPlayerState { get; private set; }
    //public E2_StunState stunState { get; private set; }
    public E5_DeadState deadState { get; private set; }
    public E5_DodgeState dodgeState { get; private set; }
    public E5_RangedAttackState rangedAttackState { get; private set; }

    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedStateData;
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

        moveState = new E5_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E5_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E5_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        meleeAttackState = new E5_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new E5_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        //stunState = new E2_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new E5_DeadState(this, stateMachine, "dead", deadStateData, this);
        dodgeState = new E5_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangedAttackState = new E5_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);

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
