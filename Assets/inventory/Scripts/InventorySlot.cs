using Maffin.InvetorySystem.Items;

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
                if (value > item.StackSize)
                    ownedAmount = item.StackSize;
                else
                    ownedAmount = value;
            }
        }
        public uint MaxAmount => item.StackSize;
    }
}