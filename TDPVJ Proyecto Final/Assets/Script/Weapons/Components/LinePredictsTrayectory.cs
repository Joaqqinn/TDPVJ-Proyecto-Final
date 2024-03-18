using System;
using Bardent.Weapons.Components;
using UnityEngine;
using Bardent.ProjectileSystem;
using Bardent.ProjectileSystem.DataPackages;
using UnityEngine.InputSystem;
using UnityEditor;
using UnityEditorInternal;

//Gestiona la linea que predice la trayectoria del lanzamineto del Axe(attack 4)
namespace Bardent.Weapons.Components
{
    public class LinePredictsTrayectory : WeaponComponent<LineTrayectoryData, AttackTrayectory>
    {
        GameObject[] points;
        public int numberOfPoints { get; private set; }

        private CoreSystem.Movement movement;
        private ProjectileSpawner projectileSpawner;
        private Player player;
        private GameObject point;
        private float spaceBetwennPoints;
        private Vector3 positionSpawner; 
        private Vector2 offset;
        private Vector2 mousePosition;
        private Vector2 direction;
        private Vector2 gravityProjectile;

        protected override void HandleEnter()
        {
            base.HandleEnter();

            point = currentAttackData.Point;
            numberOfPoints = currentAttackData.NumberOfPoints;
            spaceBetwennPoints = currentAttackData.SpaceBetwennPoints;
            points = new GameObject[numberOfPoints];
            offset = projectileSpawner.offset;
            gravityProjectile = new Vector2(0, 0);

        }
        //Seteamos la posicion que llevara cada uno de los puntos
        private Vector2 SetPointsPosition(float t)
        {
            Vector2 position = (Vector2)positionSpawner + (direction.normalized * 20 * t) + 0.5f * gravityProjectile * (t * t);
            return position;
        }
        //Actualizamos la posicion del mouse
        private void UpdatePosition()
        {
            positionSpawner = new Vector3(player.transform.position.x + (offset.x * movement.FacingDirection), player.transform.position.y + offset.y, player.transform.position.z);
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = new Vector2(mousePosition.x - positionSpawner.x, mousePosition.y - positionSpawner.y);
        }
        //Inicializamos los puntos de trayectoria
        public void InstantiatePoints(GameObject point, int i)
        {
             points[i] = point;
        }

        #region Plumbing
        protected override void Start()
        {
            base.Start();
            
            player = FindObjectOfType<Player>();
            projectileSpawner = GetComponent<ProjectileSpawner>();
            movement = Core.GetCoreComponent<CoreSystem.Movement>();
        }

        private void Update()
        {
            if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
            {
                UpdatePosition();

                for (int i = 0; i < numberOfPoints; i++)
                {
                    if (points[i] != null)
                    {
                       points[i].transform.position = SetPointsPosition(i * spaceBetwennPoints);
                    }
                }
            }
        }
        #endregion
    }
}
