using System.Collections;
using System.Collections.Generic;
using Bardent.CoreSystem;
using Bardent.Weapons;
using UnityEngine;
using System;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;

    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }

    private Movement movement;

    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }

    private CollisionSenses collisionSenses;

    protected WeaponInventory WeaponInventory
    {
        get => weaponInventory ?? core.GetCoreComponent(ref weaponInventory);
    }

    private WeaponInventory weaponInventory;

    private Weapon weapon;
    private WeaponDataSO axeWeaponDataSO;

    private bool jumpInput;
    private bool grabInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool dashInput;
    private bool slideInput;
    private bool throwInput;
    private int numberOfAttack;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,
        string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
            isTouchingCeiling = CollisionSenses.Ceiling;
        }
    }

    public override void Enter()
    {
        base.Enter();

        weapon = player.GetComponentInChildren<Weapon>();
        player.JumpState.ResetAmountOfJumpsLeft();
        player.DashState.ResetCanDash();

        foreach (var item in WeaponInventory.weaponData)
        {
            if(item.name == "AxeWeapon")
            {
                axeWeaponDataSO = item;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;
        slideInput = player.InputHandler.SlideInput;
        throwInput = player.InputHandler.ThrowInput;
        //numberOfAttack = weapon.CurrentAttackCounter;
        if (throwInput && player.InputHandler.AttackInputs[(int)CombatInputs.primary])
        {
            Debug.Log("PRESIONADO");
            weapon.SetCurrentAttackCounter();
            stateMachine.ChangeState(player.PrimaryAttackState);
            //Debug.Log();
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.primary] && !isTouchingCeiling && player.PrimaryAttackState.CanTransitionToAttackState())
        { 
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling && player.SecondaryAttackState.CanTransitionToAttackState())
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if (jumpInput && player.JumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (slideInput)
        {
            stateMachine.ChangeState(player.SlideState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}