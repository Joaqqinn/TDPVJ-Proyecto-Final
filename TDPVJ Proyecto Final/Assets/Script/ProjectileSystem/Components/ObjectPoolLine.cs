using Bardent.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Bardent.Weapons.Components;

namespace Bardent.ProjectileSystem.Components
{
    //Llama el objectpool cuando se suelta la tecla de lanzar
    public class ObjectPoolLine : ProjectileComponent
    {
        Player player;
        [SerializeField] public UnityEvent setStuck;
        protected override void Start()
        {
            base.Start();
            player = FindObjectOfType<Player>();
        }
        protected override void Update()
        {
            base.Update();
            if (!player.InputHandler.AttackInputs[(int)CombatInputs.primary])
            {
                setStuck?.Invoke();
            }
        }
    }
}
