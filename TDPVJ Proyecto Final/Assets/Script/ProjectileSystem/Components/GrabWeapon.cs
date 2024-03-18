using Bardent.Combat.Damage;
using Bardent.Utilities;
using Bardent.Weapons;
using Bardent.Weapons.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bardent.ProjectileSystem.Components
{
    public class GrabWeapon : ProjectileComponent
    {
        public UnityEvent<RaycastHit2D> OnRaycastHit;

        [field: SerializeField] public LayerMask layerMask { get; private set; }

        private Player player;

        private HitBox hitBox;

        private StickToLayer stickToLayer;

        private OutOfCamera outOfCamera;

        private Movement movement;

        private void HandleRaycastHit2D(RaycastHit2D[] hits)
        {
            if (!Active)
                return;

            foreach (var hit in hits)
            {
                // Is the object under consideration part of the LayerMask that we can damage?
                if (!LayerMaskUtilities.IsLayerInMask(hit, layerMask))
                    continue;

                // NOTE: We need to use .collider.transform instead of just .transform to get the GameObject the collider we detected is attached to, otherwise it returns the parent
                if (!hit.collider.transform.gameObject.TryGetComponent(out IDamageable damageable))
                    continue;

                MoveCamera.Instance.MovementCamera(0.8f, 10f, 0.5f);
                stickToLayer.returnToPlayer = false;
                outOfCamera.returnToPlayer = false;
                movement.ReturnToPlayer = true;

                OnRaycastHit?.Invoke(hit);

                Debug.Log("GrabWeaponState");
                player.StateMachine.ChangeState(player.GrabWeaponState);
            }
        }

        #region Plumbing

        protected override void Awake()
        {
            base.Awake();

            player = FindObjectOfType<Player>();

            hitBox = GetComponent<HitBox>();

            hitBox.OnRaycastHit2D.AddListener(HandleRaycastHit2D);

            stickToLayer = GetComponent<StickToLayer>();

            outOfCamera = GetComponent<OutOfCamera>();

            movement = GetComponent<Movement>();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            hitBox.OnRaycastHit2D.RemoveListener(HandleRaycastHit2D);
        }
        #endregion
    }
}
