using System;
using System.Collections;
using System.Collections.Generic;
using Bardent.CoreSystem;
using Bardent.Weapons;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerSlideState SlideState { get; private set; }
    public PlayerGrabWeaponState GrabWeaponState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }
    public PlayerAttackState TertiaryAttackState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }

    public Stats Stats { get; private set; }
    
    public InteractableDetector InteractableDetector { get; private set; }
    #endregion

    #region Other Variables         

    private Vector2 workspace;

    private Weapon primaryWeapon;
    private Weapon secondaryWeapon;
    private Weapon tertiaryWeapon;

    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        
        primaryWeapon = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        secondaryWeapon = transform.Find("SecondaryWeapon").GetComponent<Weapon>();
        tertiaryWeapon = transform.Find("TertiaryWeapon").GetComponent<Weapon>();

        primaryWeapon.SetCore(Core);
        secondaryWeapon.SetCore(Core);
        tertiaryWeapon.SetCore(Core);

        Stats = Core.GetCoreComponent<Stats>();
        InteractableDetector = Core.GetCoreComponent<InteractableDetector>();
        
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouch");
        SlideState = new PlayerSlideState(this, StateMachine, playerData, "slide");
        GrabWeaponState = new PlayerGrabWeaponState(this, StateMachine, playerData, "grabWeapon", primaryWeapon);
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack", primaryWeapon, CombatInputs.primary);
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack", secondaryWeapon, CombatInputs.secondary);
        TertiaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack", tertiaryWeapon, CombatInputs.tertiary);
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();

        InputHandler.OnInteractWeaponInventoy += InteractableDetector.TryInteract;

        RB = GetComponent<Rigidbody2D>();
        MovementCollider = GetComponent<BoxCollider2D>();
        
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Other Functions

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }   

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

   
    #endregion
}
