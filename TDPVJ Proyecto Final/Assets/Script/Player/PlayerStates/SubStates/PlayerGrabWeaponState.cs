using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bardent.Weapons.Components;
using Bardent.Weapons;
using Bardent.CoreSystem;
using static UnityEngine.ParticleSystem;

public class PlayerGrabWeaponState : PlayerGroundedState
{
    private Weapon weapon;
    public PlayerGrabWeaponState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, Weapon weapon) : base(player, stateMachine, playerData, animBoolName)
    {
        this.weapon = weapon;
    }
    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }

    private CollisionSenses collisionSenses;

    private ParticleManager particleManager;

    private ParticleContainer particleContainer;

    private bool isGrounded;

    public override void Enter()
    {
        base.Enter();

        particleManager = core.GetCoreComponent<ParticleManager>();
        particleContainer = core.GetCoreComponent<ParticleContainer>();

        weapon.SetProjectileInMovement(false);

        foreach (var particle in particleContainer.GetParticles())
        {
            if(particle.name == "Dust" && isGrounded)
            {
                particleManager.StartParticlesRelative(particle, particleContainer.OffsetDustParticle1, Quaternion.identity);
                particleManager.StartParticlesRelative(particle, particleContainer.OffsetDustParticle2, Quaternion.identity);
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

        if (!isExitingState)
        {
            if(isAnimationFinished)
            {
                player.StateMachine.ChangeState(player.IdleState);
            }
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
        }
    }
}
