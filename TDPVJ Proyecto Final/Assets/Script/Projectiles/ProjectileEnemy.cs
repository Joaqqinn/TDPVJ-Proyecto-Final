using UnityEngine;
using Bardent.CoreSystem;
using Bardent.Combat.Damage;
using Bardent.ObjectPoolSystem;
using Bardent.ProjectileSystem.DataPackages;
using Bardent.Weapons.Components;
using System;
using Bardent.ProjectileSystem.Components;

namespace Bardent.Projectiles
{
    public class ProjectileEnemy : MonoBehaviour
    {

        public event Action OnSetDamagePosition;

        ObjectPoolItem objectPoolItem;
        ProjectileParticles projectileParticles;

        private float speed;
        private float travelDistance;
        private float xStartPos;
        private float damageAmount;

        [SerializeField]
        private float gravity;
        [SerializeField]
        private float damageRadius;
        [SerializeField]
        private float objectPoolCooldown;

        private Rigidbody2D rb;

        private bool isGravityOn;
        private bool hasHitGround;
        private bool hasHitPlayer;

        [SerializeField]
        private LayerMask whatIsGround;
        [SerializeField]
        private LayerMask whatIsPlayer;
        [SerializeField]
        private Transform damagePosition;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            objectPoolItem = GetComponent<ObjectPoolItem>();
            projectileParticles = GetComponent<ProjectileParticles>();

            rb.gravityScale = 0.0f;
            rb.velocity = transform.right * speed;

            isGravityOn = false;

            xStartPos = transform.position.x;

            OnSetDamagePosition += SetDamagePosition;
        }

        private void OnEnable()
        {
            damagePosition.position = transform.position;
        }
        private void OnDisable()
        {
            OnSetDamagePosition -= SetDamagePosition;
        }

        private void Update()
        {
            if (!hasHitGround)
            {
                if (isGravityOn)
                {
                    float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!hasHitGround)
            {
                Collider2D[] damageHit = Physics2D.OverlapCircleAll(damagePosition.position, damageRadius, whatIsPlayer);
                Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);
                
                foreach (Collider2D collider in damageHit)
                {
                    IDamageable damageable = collider.GetComponent<IDamageable>();

                    if (damageable != null)
                    {
                        hasHitPlayer = true;
                        damageable.Damage(new DamageData(damageAmount, this.gameObject));
                        projectileParticles.SpawnImpactParticles(transform.position, transform.rotation);
                    }
                    objectPoolItem.ReturnItem(0);
                }

                if (groundHit)
                {
                    hasHitGround = true;
                    rb.gravityScale = 0f;
                    rb.velocity = Vector2.zero;
                    projectileParticles.SpawnImpactParticles(transform.position, transform.rotation);
                    objectPoolItem.ReturnItem(objectPoolCooldown);
                }


                if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
                {
                    isGravityOn = true;
                    rb.gravityScale = gravity;
                }
            }        
        }

        public void FireProjectile(float speed, float travelDistance, float damage)
        {
            Debug.Log("FIRE");
            if (hasHitGround || hasHitPlayer)
            {
                hasHitGround = false;
                hasHitPlayer = false;
                rb.gravityScale = 0.0f;
                rb.velocity = transform.right * speed;
            }
            
            this.speed = speed;
            this.travelDistance = travelDistance;
            this.damageAmount = damage;
        }

        private void SetDamagePosition()
        {
            damagePosition.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
        }
    }
}
