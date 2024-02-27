using System;
using Bardent.Weapons.Components;
using UnityEngine;
using Bardent.ProjectileSystem;
using Bardent.ProjectileSystem.DataPackages;
using UnityEngine.InputSystem;
using UnityEditor;

//Gestiona la linea que predice la trayectoria del lanzamineto del Axe(attack 4)
namespace Bardent.Weapons.Components
{
    public class LinePredictsTrayectory : WeaponComponent<LineTrayectoryData, AttackTrayectory>
    {
        GameObject[] points;
        public int numberOfPoints { get; private set; }
        
        private ProjectileSpawner projectileSpawner;
        private Player player;
        private GameObject point;
        private float spaceBetwennPoints;
        private Vector3 positionSpawner; 
        private Vector2 offset;
        private Vector2 mousePosition;
        private Vector2 direction;

        protected override void HandleEnter()
        {
            base.HandleEnter();

            point = currentAttackData.Point;
            numberOfPoints = currentAttackData.NumberOfPoints;
            spaceBetwennPoints = currentAttackData.SpaceBetwennPoints;
            points = new GameObject[numberOfPoints];
            offset = projectileSpawner.offset;
            positionSpawner = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, player.transform.position.z);

        }
        //Seteamos la posicion que llevara cada uno de los puntos
        private Vector2 SetPointsPosition(float t)
        {
            Vector2 position = (Vector2)positionSpawner + (direction.normalized * 20 * t) + 0.5f * Physics2D.gravity * (t * t);
            return position;
        }
        //Actualizamos la posicion del mouse
        private void UpdateMousePosition()
        {
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
        }

        private void Update()
        {
            if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
            {
                UpdateMousePosition();
                Debug.Log("UPDATE MOUSE");
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
