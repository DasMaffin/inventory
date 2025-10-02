using Maffin.InvetorySystem.Items;
using Maffin.InvetorySystem.Slots;

namespace Maffin.InvetorySystem.Interfaces
{
    /// <summary>
    /// An Inventory that can hold Items in Slots.
    /// </summary>
    public interface IInventory
    {
        /// <summary>
        /// Adds to any slot that has space.
        /// </summary>
        /// <param name="item">The item reference that should be added to the Inventory.</param>
        /// <param name="amount">The amount to insert into the Inventory.</param>
        /// <returns>
        /// Amount that was too big to be inserted in this Inventory. 
        /// Always 0 except when the Inventory is full!
        /// </returns>
        public uint AddItem(Item item, uint amount);

        /// <summary>
        /// Only adds to the specified slot.
        /// </summary>
        /// <param name="slot">The specific slot affected by this operation.</param>
        /// <param name="item">The item reference that should be added to the Slot.</param>
        /// <param name="amount">The amount to insert into the Slot.</param>
        /// <returns>Amount that was too big to be inserted on this Slot.</returns>
        public uint AddItem(InventorySlot slot, Item item, uint amount);

        /// <summary>
        /// Removes Item from the first slot that has the item.
        /// </summary>
        /// <param name="item">The item reference that should be removed from the Inventory</param>
        /// <param name="amount">The amount to remove from this Inventory. Should not be bigger than the return value of TotalInInventory.</param>
        /// <returns>True if Item was removed. False if Item was not removed.</returns>
        public bool RemoveItem(Item item, uint amount);

        /// <summary>
        /// Removes Item only from the specified slot.
        /// </summary>
        /// <param name="slot">The specific slot affected by this operation.</param>
        /// <param name="amount">The amount to remove from this Slot. Should not be bigger than "InventorySlot.OwnedAmount".</param>
        /// <returns>True if Item was removed. False if Item was not removed.</returns>
        public bool RemoveItem(InventorySlot slot, uint amount);

        /// <summary>
        /// Returns the total amount of the specified item in the inventory.
        /// </summary>
        /// <param name="item">The item to look for in the Inventory.</param>
        /// <returns>The total amount of the specified item in the Inventory.</returns>
        public uint TotalInInventory(Item item);
    }
}