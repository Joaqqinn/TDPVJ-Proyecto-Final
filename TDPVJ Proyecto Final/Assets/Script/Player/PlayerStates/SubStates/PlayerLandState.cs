using Bardent.CoreSystem;
using Bardent.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
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

        foreach (var particle in particleContainer.GetParticles())
        {
            if (particle.name == "Dust" && isGrounded)
            {
                particleManager.StartParticlesRelative(particle, particleContainer.OffsetDustJumpParticle, Quaternion.identity);
            }
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
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
