using System;
using System.Collections.Generic;
using Bardent.ObjectPoolSystem;
using Bardent.ProjectileSystem;
using Bardent.ProjectileSystem.Components;
using UnityEngine;

namespace Bardent.Weapons.Components
{
    public class ProjectileSpawner : WeaponComponent<ProjectileSpawnerData, AttackProjectileSpawner>
    {
        // Event fired off for each projectile before we call the Init() function on that projectile to allow other components to also pass through some data
        public event Action<Projectile> OnSpawnProjectile;

        // Movement Core Comp needed to get FacingDirection
        private CoreSystem.Movement movement;

        // Object pool to store projectiles so we don't have to keep instantiating new ones
        private readonly ObjectPools objectPools = new ObjectPools();

        // The strategy we use to spawn a projectile
        private IProjectileSpawnerStrategy projectileSpawnerStrategy;

        //LinePredictsTrayectory referencia al script
        private LinePredictsTrayectory linePredictsTrayectory;

        //Guardamos el offset que se utiliza en el punto de spawn para usarlo en la en el script LinePredictsTrayectory
        public Vector2 offset;

        public void SetProjectileSpawnerStrategy(IProjectileSpawnerStrategy newStrategy)
        {
            projectileSpawnerStrategy = newStrategy;
        }
        
        // Weapon Action Animation Event is used to trigger firing the projectiles
        private void HandleAttackAction()
        {

            foreach (var projectileSpawnInfo in currentAttackData.SpawnInfos)
            {
                //Proteccion para el attack 4 y que solo se instancie el prefab del Axe
                if (projectileSpawnInfo.ProjectilePrefab.name != "CircleTrayectory")
                {
                    // Spawn projectile based on the current strategy
                    projectileSpawnerStrategy.ExecuteSpawnStrategy(projectileSpawnInfo, transform.position,
                    movement.FacingDirection, objectPools, OnSpawnProjectile);
                }
            }
        }

        //Ejecutamos cuando comienza la animacion de anticipation
        private void HandleLineTrayectory()
        {
            foreach (var projectileSpawnInfo in currentAttackData.SpawnInfos)
            {
                //Proteccion para el attack 4 y que solo se instancie el prefab del Point
                if(projectileSpawnInfo.ProjectilePrefab.name == "CircleTrayectory")
                {
                    // Spawn projectile based on the current strategy
                    projectileSpawnerStrategy.ExecuteSpawnStrategy(projectileSpawnInfo, transform.position,
                        movement.FacingDirection, objectPools, OnSpawnProjectile);
                    //Instanciamos 10 CircleTrayectory con la stategy de objectpool
                    for (int i = 0; i < 10; i++)
                    {
                        //Instanciamos los puntos de trayectoraria y los pasamos a su script
                        linePredictsTrayectory.InstantiatePoints(objectPools.GetPool(projectileSpawnInfo.ProjectilePrefab).GetObject().gameObject, i);
                    }   
                }
            }
        }
        private void SetDefaultProjectileSpawnStrategy()
        {
            // The default spawn strategy is the base ProjectileSpawnerStrategy class. It simply spawns one projectile based on the data per request
            projectileSpawnerStrategy = new ProjectileSpawnerStrategy();
        }

        protected override void HandleExit()
        {
            base.HandleExit();
            
            // Reset the spawner strategy every time the attack finishes in case some other component adjusted it
            SetDefaultProjectileSpawnStrategy();
        }

        #region Plumbing

        protected override void Awake()
        {
            base.Awake();
            
            SetDefaultProjectileSpawnStrategy();
        }

        protected override void Start()
        {
            base.Start();

            movement = Core.GetCoreComponent<CoreSystem.Movement>();
            linePredictsTrayectory = GetComponent<LinePredictsTrayectory>();

            AnimationEventHandler.OnAttackAction += HandleAttackAction;
            AnimationEventHandler.OnSpawnTrayectory += HandleLineTrayectory;

            GetOffset();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            AnimationEventHandler.OnAttackAction -= HandleAttackAction;
            AnimationEventHandler.OnSpawnTrayectory -= HandleLineTrayectory;
        }

        private void GetOffset()
        {
            if (data == null || !Application.isPlaying)
                return;

            foreach (var item in data.GetAllAttackData())
            {
                foreach (var point in item.SpawnInfos)
                {
                    offset = point.OffsetData.Offset;
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (data == null || !Application.isPlaying)
                return;

            foreach (var item in data.GetAllAttackData())
            {
                foreach (var point in item.SpawnInfos)
                {

                    var pos = transform.position + (Vector3)point.OffsetData.Offset;
                    Gizmos.DrawWireSphere(pos, 0.2f);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(pos, pos + (Vector3)point.Direction.normalized);
                    Gizmos.color = Color.white;
                }
            }
        }

        #endregion
    }
}