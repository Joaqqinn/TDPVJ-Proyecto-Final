using Bardent.ObjectPoolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Bardent.Weapons.Components;

//Encargado de ejecutar el objectpooling en la linea de trayectoria cuando se suelta la tecla de lanzar 
namespace Bardent.ProjectileSystem.Components
{
    public class ObjectPoolTrayectory : ProjectileComponent
    {
        Player player;
        ObjectPoolItem objectPoolItem;
        [SerializeField] public UnityEvent setStuck;
        protected override void Start()
        {
            base.Start();
            player = FindObjectOfType<Player>();
            objectPoolItem = GetComponent<ObjectPoolItem>();
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
