using Bardent.ObjectPoolSystem;
using Bardent.ProjectileSystem.DataPackages;
using Bardent.Weapons;
using Bardent.Weapons.Components;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bardent.ProjectileSystem.Components
{
    /// <summary>
    /// The Movement projectile component is responsible for applying a velocity to the projectile. The velocity can be applied only once upon the projectile
    /// being fired, or can be applied continuously as if self powered. 
    /// </summary>
    public class Movement : ProjectileComponent
    {
        [field: SerializeField] public bool ApplyContinuously { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public bool ThrowTheMouse { get; private set; }
        [field: SerializeField] public bool ReturnToPlayer { get; set; }
        [field: SerializeField] public float ReturnToPlayerCounter { get; private set; }
        [field: SerializeField] public bool ReturnTheWeaponNotVisible { get; private set; }

        

        Player player;
        [NonSerialized] public Vector3 projectilePosition;

        StickToLayer stickToLayer;

        OutOfCamera outOfCamera;

        private Vector3 playerPosition;
        private Vector2 mousePosition;
        private Vector3 direction;
        private Vector2 offset;
        
        private void SetVelocity() => rb.velocity = Speed * transform.right;
        private void SetThrowTheMouse()
        {
            //Debug.Log("SET TO MOUSE");
            projectilePosition = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, player.transform.position.z);
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = new Vector3(mousePosition.x - projectilePosition.x, mousePosition.y - projectilePosition.y, projectilePosition.z);
            transform.right = direction;
        }
        private void SetReturnToPlayer()
        {
            //Debug.Log("RETUR TO PLAYER");
            playerPosition = player.transform.position;
            direction = new Vector3(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y, transform.position.z);
            transform.right = direction;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (ReturnToPlayer && stickToLayer.returnToPlayer)
            {
                //Debug.Log("RETURN stickToLayer");

                stickToLayer.SetUnstuck();
                SetReturnToPlayer();
                SetVelocity();
            }

            if (ReturnToPlayer && outOfCamera.returnToPlayer)
            {
                //Debug.Log("RETURN outOfCamera");

                SetReturnToPlayer();
                SetVelocity();
            }

            if (!ApplyContinuously)
                return;

            SetVelocity();
        }
        protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
        {
            base.HandleReceiveDataPackage(dataPackage);

            if (!(dataPackage is OffsetDataPackage package))
            return;

            offset = package.Offset;
        }


        #region Plumbing


        // On Init, set projectile velocity once
        protected override void Init()
        {
            base.Init();

            player = FindObjectOfType<Player>();
            stickToLayer = GetComponent<StickToLayer>();
            outOfCamera = GetComponent<OutOfCamera>();

            if (ThrowTheMouse) { SetThrowTheMouse(); }

            SetVelocity();
        }

        protected override void Update()
        {
            base.Update();

        }

        #endregion
    }
}