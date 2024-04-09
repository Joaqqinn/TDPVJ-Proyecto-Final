using UnityEngine;
using Bardent.CoreSystem;
using Bardent.Combat.Damage;
using Bardent.ObjectPoolSystem;
using Bardent.ProjectileSystem.DataPackages;
using Bardent.Weapons.Components;
using System;

namespace Bardent.Projectiles
{
    public class ProjectileEnemy : MonoBehaviour
    {
        ObjectPoolItem objectPoolItem;

        private float speed;
        private float travelDistance;
        private float xStartPos;
        private float damageAmount;

        [SerializeField]
        private float gravity;
        [SerializeField]
        private float damageRadius;

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
            
            rb.gravityScale = 0.0f;
            rb.velocity = transform.right * speed;

            isGravityOn = false;

            xStartPos = transform.position.x;
        }

        private void Update()
        {
            if (!hasHitGround)
            {
                Debug.Log("MOVE0");
                if (isGravityOn)
                {
                    Debug.Log("MOVE");
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
                    }
                    objectPoolItem.ReturnItem(0);
                }

                if (groundHit)
                {
                    hasHitGround = true;
                    rb.gravityScale = 0f;
                    rb.velocity = Vector2.zero;
                    objectPoolItem.ReturnItem(3);
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

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
        }
    }
}
