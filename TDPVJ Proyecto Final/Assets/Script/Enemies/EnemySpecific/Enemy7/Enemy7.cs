using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy7 : Entity
{
    public E7_PlayerDetectedState playerDetectedState { get; private set; }
    public E7_ChargeState chargeState { get; private set; }
    public E7_MeleeAttackState meleeAttackState { get; private set; }
    public E7_LookForPlayerState lookForPlayerState { get; private set; }
    public E7_FlyingState flyingState { get; private set; }

    [SerializeField]
    private D_PlayerDetected playerDetectedStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_FlyingState flyingStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;


    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private GameObject routePoints;

    public override void Awake()
    {
        base.Awake();

        chargeState = new E7_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        playerDetectedState = new E7_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        flyingState = new E7_FlyingState(this, stateMachine, "flying", routePoints, flyingStateData, this);
        meleeAttackState = new E7_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new E7_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
    }

    protected override void HandleParry()
    {
        base.HandleParry();

        Debug.Log("PARRY STUN STATE");
        //stateMachine.ChangeState(stunState);
    }

    private void Start()
    {
        stateMachine.Initialize(flyingState);        
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
