using Bardent.Utilities;
using System;
using UnityEngine;

namespace Bardent.ProjectileSystem.Components
{
    public class ProjectileParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem impactParticles;
        [field: SerializeField] public LayerMask LayerMask { get; private set; }
        //Se encarga de controlar la ejecucion de las particulas una sola vez cuando un proyectil no se destruye al colisionar
        [NonSerialized] public bool runOneTime = false;

        public void SpawnImpactParticles(Vector3 position, Quaternion rotation)
        {
            Instantiate(impactParticles, position, rotation);
        }

        public void SpawnImpactParticles(RaycastHit2D hit)
        {
            var rotation = Quaternion.FromToRotation(transform.right, hit.normal);
            
            SpawnImpactParticles(hit.point, rotation);
        }

        public void SpawnImpactParticles(RaycastHit2D[] hits)
        {
            if(hits.Length <= 0 )
                return;

            foreach (var hit in hits)
            {
                // Is the object under consideration part of the LayerMask that we can damage?
                if (!LayerMaskUtilities.IsLayerInMask(hit, LayerMask))
                    continue;

                SpawnImpactParticles(hits[0]);
            }
        }
    }
}