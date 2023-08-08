using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SorcererRush
{
    [AddComponentMenu("Game/PlayerUnit")]
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerUnit : Character
    {
        public PlayerControl control { get; private set; }

        [SerializeField] private PlayerUnitStats playerStats;

        public override Stats GetStats()
        {
            return playerStats;
        }

        protected override void Awake()
        {
            base.Awake();
            control = GetComponent<PlayerControl>();
            control.InitValues();
        }

        public override void OnDeath()
        {
            base.OnDeath();
            if (playerUnits.Count == 0)
            {
                GameStartPoint.Instance.GameOver();
            }
        }

        private void FixedUpdate()
        {
            control.Move();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            playerStats = (GetPrefab().character as PlayerUnit).playerStats;
        }
    }
}