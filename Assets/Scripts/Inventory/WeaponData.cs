using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SorcererRush
{
    [CreateAssetMenu(fileName = "Weapon Data", menuName = "WeaponData", order = 0)]
    public class WeaponData : ItemData
    {
        public bool isPlayerWeapon = true;
        public int maxLevel;
        public int startLevel;

        public WeaponStats baseStats;
        public List<WeaponLevelStats> stats = new();

        public ComponentsCach projectile;
        public TargetMode targetMode = TargetMode.Nearest;
        // public AttackMode attackMode;
        //
        // public bool IsProjectileAttack => attackMode == AttackMode.Projectile;
        // public bool IsTouchAttack => attackMode == AttackMode.Touch;

        public WeaponLevelStats CurrentStats
        {
            get
            {
                WeaponLevelStats result = baseStats.As_WeaponLevelStats();
                result.level = stats.Last().level;
                foreach (var stat in stats)
                {
                    result.permanent = stat.permanent || result.permanent;
                    result.speed += stat.speed;
                    result.damage += stat.damage;
                    result.cooldown += stat.cooldown;
                    result.size += stat.size;
                    result.duration += stat.duration;
                }

                return result;
            }
        }
    }

    public enum TargetMode
    {
        Nearest,
        Random
    }
}