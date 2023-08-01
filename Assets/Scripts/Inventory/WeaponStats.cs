using System;

namespace SorcererRush
{
    [Serializable]
    public class WeaponStats : Stats
    {
        public bool permanent = false;
        public float speed = 0.0f;
        public float damage = 0.0f;
        public float cooldown = 0.0f;
        public float size = 0.0f;
        public float duration = 0.0f;

        public WeaponLevelStats As_WeaponLevelStats()
        {
            return new WeaponLevelStats()
            {
                duration = duration,
                cooldown = cooldown,
                damage = damage,
                permanent = permanent,
                size = size,
                speed = speed
            };
        }
    }
}