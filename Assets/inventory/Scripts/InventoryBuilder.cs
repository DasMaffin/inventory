using Maffin.InvetorySystem.Inventories;
using Maffin.InvetorySystem.Slots;
using UnityEngine;

namespace Maffin.InvetorySystem.Builders
{
    /// <summary>
    /// Inventory builder. Use InventoryBuilder.Build() to create a new inventory.
    /// </summary>
    /// <example>
    /// InventoryBuilder.Create()
    ///     .SetOwner(owner)
    ///     .Build()
    /// </example>
    public class InventoryBuilder
    {
        private GameObject
            owner;    // The owner of the inventory.
        private uint
            capacity = 0;    // How many slots in the inventory.
        private bool
            canOpen = true;
        private InventorySlot[]
            slots;    // The slots in the inventory.

        public InventoryBuilder() { }

        public static InventoryBuilder Create()
        {
            return new InventoryBuilder();
        }

        /// <summary>
        /// Creates a new inventory instance.
        /// </summary>
        /// <returns>Returns the new inventory instance.</returns>
        public Inventory Build()
        {
            return new Inventory(owner, capacity, canOpen, slots);
        }

        /// <summary>
        /// Sets the owner of the inventory. This is usually the player or entity that holds the inventory.
        /// </summary>
        /// <param name="owner">The object owning the inventory.</param>
        public InventoryBuilder SetOwner(GameObject owner)
        {
            if (owner == null)
            {
                Debug.LogWarning("Inventory owner is null. This is probably unintended and should be fixed.\nIf this is intended remove this method from the inventory.");
                return this;
            }
            this.owner = owner;
            return this;
        }

        /// <summary>
        /// The amount of slots in the inventory.
        /// </summary>
        /// <param name="capacity">The amount of slots in the inventory.</param>
        public InventoryBuilder SetCapacity(uint capacity)
        {
            this.capacity = capacity;
            this.slots = new InventorySlot[capacity];
            for(int i = 0; i < capacity; i++)
            {
                this.slots[i] = new InventorySlot(null, 0);
            }
            return this;
        }

        /// <summary>
        /// Whether or not the inventory can be opened. Default is true.
        /// If true the inventory can be opened by clicking on its owner.
        /// </summary>
        /// <param name="canOpen"></param>
        /// <returns></returns>
        public InventoryBuilder SetCanOpen(bool canOpen)
        {
            this.canOpen = canOpen;
            return this;
        }
    }
}