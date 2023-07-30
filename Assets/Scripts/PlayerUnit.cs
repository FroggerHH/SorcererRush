using System;
using UnityEngine;

namespace SorcererRush
{
    [AddComponentMenu("Game/PlayerUnit")]
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerUnit : Unit
    {
        public PlayerControl control { get; private set; }

        private void Awake()
        {
            control = GetComponent<PlayerControl>();
            control.InitValues();
        }

        public override void OnDamaged()
        {
        }

        private void FixedUpdate()
        {
            control.Move();
        }
    }
}