using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace SorcererRush
{
    [Serializable]
    public class Weapon : InventoryItem
    {
        public int currentLevel;
        public WeaponData data;
        public Character owner;

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

            return true;
        }

        private bool ValidateWeaponData() => data != null && data.projectile != null;

        public float CalculateLifeTime()
        {
            var result = data.baseStats.duration * (1 + data.CurrentStats.duration);
            if (owner.GetStats() is PlayerUnitStats playerStats) result *= playerStats.duration;
            return result;
        }
    }
}