﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //public event Action<bool> OnInteractInputChanged;//
    public event Action<bool> OnInteractWeaponInventoy;

    private PlayerInput playerInput;
    private Camera cam;

    public Vector2 RawMovementInput { get; private set; }
    //public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }
    public Vector2Int SlideDirectionInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool SlideInput { get; private set; }
    public bool SlideInputStop { get; private set; }
    public bool[] AttackInputs { get; private set; }
    public bool ThrowInput { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;
    private float slideInputStartTime;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];

        cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
        CheckSlideInputHoldTime();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInteractWeaponInventoy?.Invoke(true);
            return;
        }

        if (context.canceled)
        {
            OnInteractWeaponInventoy?.Invoke(false);
        }
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.primary] = false;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.secondary] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.secondary] = false;
        }
    }
    public void OntTertiaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.tertiary] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.tertiary] = false;
        }
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);       
        
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;

            SlideInputStop = false;
            slideInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
            SlideInputStop = true;
        }
    }
    public void OnThrowInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ThrowInput = true;
            Debug.Log("PRESIONADO ALT");
        }
        else if (context.canceled)
        {
            ThrowInput = false;
        }
    }


    public void UseJumpInput() => JumpInput = false;

    public void UseDashInput() => DashInput = false;
    public void UseSlideInput() => SlideInput = false;

    /// <summary>
    /// Used to set the specific attack input back to false. Usually passed through the player attack state from an animation event.
    /// </summary>
    public void UseAttackInput(int i) => AttackInputs[i] = false;

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if(Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }

    private void CheckSlideInputHoldTime()
    {
        if (Time.time >= slideInputStartTime + inputHoldTime)
        {
            SlideInput = false;
        }
    }
}

public enum CombatInputs
{
    primary,
    secondary,
    tertiary
}
