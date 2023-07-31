using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SorcererRush
{
    [AddComponentMenu("Game/PlayerUnit")]
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerUnit : Character
    {
        public static List<PlayerUnit> playerUnits { get; private set; } = new();
        public PlayerControl control { get; private set; }

        [SerializeField] private PlayerUnitStats playerStats;

        public override Stats GetStats()
        {
            return playerStats;
        }

        private void Awake()
        {
            control = GetComponent<PlayerControl>();
            control.InitValues();
        }

        public override void OnDeath()
        {
            base.OnDeath();
            if (playerUnits.Count == 0)
            {
                GameManager.Instance.GameOver();
            }
        }

        private void FixedUpdate()
        {
            control.Move();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            playerUnits.Add(this);
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            playerUnits.Remove(this);
        }
    }
}