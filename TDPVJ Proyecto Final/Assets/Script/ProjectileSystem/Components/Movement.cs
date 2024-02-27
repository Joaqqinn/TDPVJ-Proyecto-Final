using Bardent.ObjectPoolSystem;
using Bardent.ProjectileSystem.DataPackages;
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

        Player player;
        Camera cam;
        [NonSerialized] public Vector3 projectilePosition;

        private Vector2 mousePosition;
        private Vector3 direction;
        private Vector2 offset;

        // On Init, set projectile velocity once
        protected override void Init()
        {
            base.Init();

            player = FindObjectOfType<Player>();
            cam = Camera.main;

            if (ThrowTheMouse) { SetThrowTheMouse(); }
            SetVelocity();
        }
        private void SetVelocity() => rb.velocity = Speed * transform.right;
        private void SetThrowTheMouse()
        {
            projectilePosition = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, player.transform.position.z);
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = new Vector3(mousePosition.x - projectilePosition.x, mousePosition.y - projectilePosition.y, projectilePosition.z);
            transform.right = direction;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (!ApplyContinuously)
                return;

            if (ThrowTheMouse) { SetThrowTheMouse(); }
            SetVelocity();
        }
        protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
        {
            base.HandleReceiveDataPackage(dataPackage);

            if (!(dataPackage is OffsetDataPackage package))
            return;

            offset = package.Offset;
        }
    }
}