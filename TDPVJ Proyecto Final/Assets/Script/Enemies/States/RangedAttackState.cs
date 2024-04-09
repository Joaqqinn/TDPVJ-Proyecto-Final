using System.Collections;
using System.Collections.Generic;
using Bardent.ObjectPoolSystem;
using Bardent.ProjectileSystem.DataPackages;
using Bardent.Projectiles;
using UnityEngine;

public class RangedAttackState : AttackState
{
    private DamageDataPackage DamageDataPackage;

    private ObjectPools objectPools = new ObjectPools();

    protected D_RangedAttackState stateData;
    
    protected GameObject projectile;
    protected ProjectileEnemy projectileScript;

    public RangedAttackState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangedAttackState stateData) : base(etity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        /*projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, stateData.projectileDamage);*/

        var projectile = objectPools.GetPool(stateData.projectile).GetObject();

        projectile.Reset();

        projectile.transform.position = attackPosition.position;
        projectile.transform.rotation = attackPosition.rotation;

        projectile.GetComponent<ProjectileEnemy>().FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, stateData.projectileDamage);

        projectile.SendDataPackage(DamageDataPackage);

        projectile.Init();
    }
}
