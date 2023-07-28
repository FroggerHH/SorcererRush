using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SorcererRush
{
    public class Spell : IDamageSource
    {
        HitInfo hitInfo;

        public HitInfo GetHitInfo()
        {
            return hitInfo;
        }

        public virtual void ApplyDamageTo(ITakeDamage target)
        {
            target.TakeDamage(hitInfo.firstHitDamage);
            Coroutines.Start(TakeTickDamage(target, hitInfo.tickDamage));
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