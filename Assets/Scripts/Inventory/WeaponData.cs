using System.Collections.Generic;
using UnityEngine;

namespace SorcererRush
{
    [CreateAssetMenu(fileName = "Weapon Data", menuName = "WeaponData", order = 0)]
    public class WeaponData : ItemData
    {
        public int maxLevel;
        public int startLevel;

        public AttackMode attackMode;
        public ComponentsCach projectile;
        public WeaponStats baseStats;

        public List<WeaponLevelStats> stats = new();
        public bool IsProjectileAttack => attackMode == AttackMode.Projectile;
        public bool IsTouchAttack => attackMode == AttackMode.Touch;
    }
}