using System;

namespace SorcererRush
{
    [Serializable]
    public class PlayerUnitStats : Stats
    {
        public PlayerUnitStats(UnitStats unitStats)
        {
            this.unitStats = unitStats;
        }

        public UnitStats unitStats { get; private set; }
        public float cooldown = 0.0f;
        public float duration = 0.0f;
    }
}