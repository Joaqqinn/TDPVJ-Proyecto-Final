using System;
using UnityEngine;

namespace Bardent.Weapons.Components
{
    [Serializable]
    public class AttackTrayectory : AttackData
    {
        [field: SerializeField] public GameObject Point { get; private set; }
        [field: SerializeField] public int NumberOfPoints { get; private set; }
        [field: SerializeField] public float SpaceBetwennPoints { get; private set; }
    }
}