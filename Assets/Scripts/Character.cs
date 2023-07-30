using System;
using UnityEngine;

namespace SorcererRush
{
    [AddComponentMenu("Game/Character")]
    public class Character : Unit
    {
        public override void OnDamaged()
        {
            base.OnDamaged();
            Utils.Heightlight(this, Color.red);
        }
    }
}