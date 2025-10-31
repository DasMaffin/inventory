using Maffin.InvetorySystem.Items;
using UnityEngine;

namespace Maffin.InvetorySystem.Slots
{
    public class InventorySlot
    {
        private Item
            item;
        private uint
            ownedAmount;

        public Item Item 
        {
            get => item; 
            set
            {
                item = value;
                if(item == null)
                    ownedAmount = 0;
            }
        }
        public uint OwnedAmount
        {
            get => ownedAmount;
            set
            {
                if (item != null && value > item.StackSize)
                    ownedAmount = item.StackSize;
                else
                    ownedAmount = value;
            }
        }
        public uint MaxAmount => item.StackSize;
        public GameObject placeholder;

        private bool keepItem = false;
        public bool KeepItem
        {
            get => keepItem;
            set
            {
                keepItem = value;
            }
        }

        public InventorySlot(Item item, uint ownedAmount)
        {
            this.item = item;
            this.ownedAmount = ownedAmount;
        }
    }
}