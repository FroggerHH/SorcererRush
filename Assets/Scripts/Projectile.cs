using UnityEngine;
using NaughtyAttributes;
using NTC.Global.Pool;

namespace SorcererRush
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;

        public void Setup(Vector2 direction)
        {
            NightPool.Despawn(this, weapon.CalculateLifeTime());
        }
    }
}