using System;
using UnityEngine;

namespace Bardent.ProjectileSystem.DataPackages
{
    [Serializable]
    public class OffsetDataPackage : ProjectileDataPackage
    {
        [field: SerializeField] public Vector2 Offset { get; private set; }
    }
}