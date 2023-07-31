using System.Linq;
using IngameDebugConsole;
using Redcode.Extensions;
using UnityEngine;

namespace SorcererRush
{
    public static class Commands
    {
        [ConsoleMethod(nameof(KillAll), "Insantly kills all enemies")]
        public static void KillAll()
        {
            var units = Character.Instances.Where(x => x.fraction is UnitFraction.Monster).ToList();
            var unitsCount = units.Count;
            units.ForEach(x => x.TakeDamage(new Damage(int.MaxValue, DamageType.DevDamage)));
            Debug.Log($"{unitsCount} enemies have been killed");
        }

        [ConsoleMethod(nameof(SetPlayerSpeed), "Set players speed")]
        public static void SetPlayerSpeed(float value)
        {
            PlayerUnit.Instances.Where(x => x.fraction is UnitFraction.Player)
                .ForEach(x => (x as PlayerUnit).control.SetSpeed(value));
        }
    }
}