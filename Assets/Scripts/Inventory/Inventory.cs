using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace SorcererRush
{
    [Serializable]
    public class Inventory
    {
        [SerializeField] [ReadOnly] private List<InventoryItem> items = new();

        public List<InventoryItem> GetItems() => items;

        public List<InventoryItem> GetItems(ItemType itemType) =>
            items.Where(x => x.item.itemType == itemType).ToList();

        public void AddItem(InventoryItem item)
        {
            if (!items.Contains(item)) items.Add(item);
        }

        public void AddItem(ItemData item)
        {
            InventoryItem inventoryItem = new(item);
            if (!items.Contains(inventoryItem)) items.Add(inventoryItem);
        }

        public void AddWeapon(Weapon weapon) => AddItem(weapon);

        public Weapon GetRandomWeapon() => GetItems(ItemType.Weapon).Random() as Weapon;
    }
}