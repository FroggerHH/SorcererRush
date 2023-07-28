using System;

namespace SorcererRush
{
    [Serializable]
    public class TickDamage : Damage
    {
        public TickDamage(float damage, DamageType damageType, float tickInterval, int tickCount) : base(damage,
            damageType)
        {
            this.tickInterval = tickInterval;
            this.tickCount = tickCount;
        }

        public float tickInterval = .5f;
        public int tickCount = 1;
    }
}