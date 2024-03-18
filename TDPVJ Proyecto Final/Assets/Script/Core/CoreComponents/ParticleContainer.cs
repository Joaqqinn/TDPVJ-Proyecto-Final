using UnityEngine;

namespace Bardent.CoreSystem
{
    public class ParticleContainer : CoreComponent
    {
        [SerializeField] private GameObject[] particles;

        [field: SerializeField] public Vector2 OffsetDustParticle1 { get; private set; }
        [field: SerializeField] public Vector2 OffsetDustParticle2 { get; private set; }
        [field: SerializeField] public Vector2 OffsetDustJumpParticle { get; private set; }
        [field: SerializeField] public Vector2 OffsetDustSlideParticle { get; private set; }

        public GameObject[] GetParticles()
        {
            return particles;
        }
    }
}