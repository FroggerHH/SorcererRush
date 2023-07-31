using System;

namespace SorcererRush
{
    [Serializable]
    public class InventoryItem
    {
        public ItemData item;

        public InventoryItem(ItemData item)
        {
            this.item = item;
        }
    }
}
