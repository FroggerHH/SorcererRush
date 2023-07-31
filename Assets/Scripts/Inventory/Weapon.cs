using System;
using System.Collections.Generic;
using NaughtyAttributes;

namespace SorcererRush
{
    [Serializable]
    public class Weapon : InventoryItem
    {
        public int currentLevel;
        public WeaponData data;

        public Weapon(WeaponData item) : base(item){}
    }
}