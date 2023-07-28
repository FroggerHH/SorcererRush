using System;
using UnityEngine.Serialization;

namespace SorcererRush
{
    [Serializable]
    public class HitInfo
    {
        public Damage firstHitDamage;
        public TickDamage tickDamage;

        public bool HasTickDamage()
        {
            return tickDamage != null;
        }
    }
}