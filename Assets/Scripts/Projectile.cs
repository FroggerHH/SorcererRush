using System;
using UnityEngine;
using NaughtyAttributes;
using NTC.Global.Pool;

namespace SorcererRush
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IPoolItem
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private Rigidbody rb;

        public void Setup(Vector2 direction)
        {
            NightPool.Despawn(this, weapon.CalculateLifeTime());
            rb.AddForce(new(direction.x, 0, direction.y), ForceMode.Impulse);
        }

        public void OnSpawn()
        {
            rb.velocity = new();
        }

        public void OnDespawn()
        {
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag(GameManager.Instance.enemyTag)) return;
            if(!other.gameObject.TryGetComponent(out ITakeDamage takeDamage)) return;
            weapon.ApplyDamageTo(takeDamage);
        }
    }
}