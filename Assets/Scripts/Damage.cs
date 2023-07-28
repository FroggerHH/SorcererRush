using System;

namespace SorcererRush
{
    [Serializable]
    public class Damage
    {
        public Damage(float damage, DamageType damageType)
        {
            this.damage = damage;
            this.damageType = damageType;
        }

        public float damage = 0;
        public DamageType damageType = DamageType.Physical;
    }
}