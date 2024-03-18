using Bardent.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bardent.ProjectileSystem.Components
{
    public class OutOfCamera : ProjectileComponent
    {
        [SerializeField] public UnityEvent setStuck;
        [field: SerializeField] public float TimeSpawnToDeactivate { get; private set; }

        private Movement movement;

        private StickToLayer stickToLayer;
        
        private TimeNotifier timeNotifier;

        [NonSerialized] public bool returnToPlayer;
        private void ActiveObjectPool()
        {
            if (!movement.ReturnTheWeaponNotVisible) { setStuck?.Invoke(); }
            else
            {
                returnToPlayer = true;
            }
        }
        #region Plumbing
        protected override void Awake()
        {            
            movement = GetComponent<Movement>();

            stickToLayer = GetComponent<StickToLayer>();

            timeNotifier = new TimeNotifier();

            returnToPlayer = false;
        }

        protected override void Update()
        {
            base.Update();

            if (!stickToLayer.isStuck && !stickToLayer.returnToPlayer) { timeNotifier.Tick(); }
        }
        private void OnEnable()
        {
            if (!stickToLayer.isStuck && !stickToLayer.returnToPlayer) { timeNotifier.Init(TimeSpawnToDeactivate); }
            timeNotifier.OnNotify += ActiveObjectPool;
        }

        private void OnDisable()
        {
            if (!stickToLayer.isStuck && !stickToLayer.returnToPlayer) { timeNotifier.Disable(); }
            timeNotifier.OnNotify -= ActiveObjectPool;    
        }

        #endregion
    }
}
