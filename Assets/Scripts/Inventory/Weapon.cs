using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NTC.Global.Pool;
using UnityEngine;

namespace SorcererRush
{
    [Serializable]
    public class Weapon : InventoryItem, IDamageSource
    {
        public int currentLevel;
        public WeaponData data;
        public Character owner;

        private HitInfo hitInfo;

        public Weapon(WeaponData item, Character owner) : base(item)
        {
            this.owner = owner;
        }

        public bool DoAttack(ITakeDamage target)
        {
            if (!ValidateWeaponData())
            {
                Debug.LogError("Weapon data is not valid!");
                return false;
            }

            var spawnPosition = GetSpawnPosition();
            var spawn = NightPool.Spawn(data.projectile, spawnPosition).projectile;
            var dir = (target.GetPosition() - spawnPosition).normalized;
            spawn.Setup(dir);

            return true;
        }

        private Vector2 GetSpawnPosition()
        {
            return data.targetMode switch
            {
                TargetMode.Nearest => new(),
                TargetMode.Random => new(),
                _ => new()
            };
        }

        private bool ValidateWeaponData() => data != null && data.projectile != null;

        public float CalculateLifeTime()
        {
            var result = data.baseStats.duration * (1 + data.CurrentStats.duration);
            if (owner.GetStats() is PlayerUnitStats playerStats) result *= playerStats.duration;
            return result;
        }
        
        public HitInfo GetHitInfo()
        {
            return hitInfo;
        }

        public virtual void ApplyDamageTo(ITakeDamage target)
        {
            target.TakeDamage(hitInfo.firstHitDamage);
            target.GetMonoBehaviour().StartCoroutine(TakeTickDamage(target, hitInfo.tickDamage));
        }

        protected IEnumerator TakeTickDamage(ITakeDamage target, TickDamage tickDamage)
        {
            for (int i = 0; i < tickDamage.tickCount; i++)
            {
                yield return new WaitForSeconds(tickDamage.tickInterval);
                target.TakeDamage(tickDamage);
            }
        }
    }
}