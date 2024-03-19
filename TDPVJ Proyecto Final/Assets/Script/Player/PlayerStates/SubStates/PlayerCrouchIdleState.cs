using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bardent.Weapons;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    private Weapon weapon;

    private bool throwInput;
    public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        weapon = player.GetComponentInChildren<Weapon>();

        Movement?.SetVelocityZero();
        //player.SetColliderHeight(playerData.crouchColliderHeight);
    }

    public override void Exit()
    {
        base.Exit();
        //player.SetColliderHeight(playerData.standColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.CheckIfShouldFlip(player.InputHandler.NormInputX);
        throwInput = player.InputHandler.ThrowInput;

        if (!isExitingState)
        {
           if(yInput != -1 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.IdleState);
            }
           else if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
            {
                weapon.SetCurrentAttackCounter(3);
                stateMachine.ChangeState(player.PrimaryAttackState);
            }
        }
    }
}
