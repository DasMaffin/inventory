using Maffin.InvetorySystem.Interfaces;
using Maffin.InvetorySystem.Items;
using Maffin.InvetorySystem.Slots;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maffin.InvetorySystem.Inventories
{
    public class Inventory : IInventory
    {
        private GameObject
            owner;    // The owner of the inventory.
        private uint
            capacity = 0;
        private int
            hotbarSelectedIndex = 0;
        public bool
            canOpen = true,
            isLocalPlayerInventory = false;
        public InventorySlot[]
            slots;
        private InventorySlot _selectedSlot;
        private InventorySlot selectedSlot
        {
            get => _selectedSlot;
            set
            {
                _selectedSlot = value;

                OnSelectedSlotChanged?.Invoke(selectedSlot);
            }
        }

        private static string ItemsPath;
        private static List<Item> allItems = new List<Item>();

        public event Action<InventorySlot> OnInventoryChanged;
        private event Action<InventorySlot> OnSelectedSlotChanged;

        public static List<Item> AllItems
        {
            get
            {
                if (allItems.Count == 0)
                {
                    allItems.AddRange(Resources.LoadAll<Item>(ItemsPath));
                }
                return allItems;
            }
        }

        public static void SetItemsPath(string path)
        {
            ItemsPath = path;
            allItems.Clear();
        }

        internal Inventory(GameObject owner, uint capacity, bool canOpen, InventorySlot[] slots, bool _isLocalPlayerInventory)
        {
            this.owner = owner;
            this.capacity = capacity;
            this.canOpen = canOpen;
            this.slots = slots;
            this.isLocalPlayerInventory = _isLocalPlayerInventory;
            if(this.isLocalPlayerInventory)
                this.selectedSlot = this.slots[0];
            else
                this.selectedSlot = null;
        }

        public uint AddItem(Item item, uint amount)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item does not exist!");
            
            while(amount > 0)
            {
                // Find a slot that either has the same item, and space, or an empty slot. Prioritize existing stacks.
                // Improve this by indexing slots by item type wiht a Dictionary. Would take time from O(n) to O(1). However, only necessary for large inventories and I am lazy.
                InventorySlot slot = 
                    slots.FirstOrDefault(s => s.Item == item && s.OwnedAmount < s.MaxAmount) ??
                    slots.FirstOrDefault(s => s.Item == null);
                if(slot == null) break; // Inventory full
                amount = AddItem(slot, item, amount);
            }
            return amount;
        }

        public uint AddItem(InventorySlot slot, Item item, uint amount)
        {
            if (
                slot == null ||
                item == null ||
                (slot.Item != null && slot.Item != item))
                return amount;

            slot.Item = item;
            uint freeSpace = slot.MaxAmount - slot.OwnedAmount;
            if (amount > freeSpace)
            {
                amount -= freeSpace;
                slot.OwnedAmount = slot.MaxAmount;
            }
            else
            {
                slot.OwnedAmount += amount;
                OnInventoryChanged?.Invoke(slot);
                return 0;
            }
            OnInventoryChanged?.Invoke(slot);
            return amount;
        }

        public uint RemoveItem(Item item, uint amount)
        {
            if (
                item == null ||
                TotalInInventory(item) < amount)
                return amount;

            while(amount > 0)
            {
                InventorySlot slot = slots.FirstOrDefault(s => s.Item == item);
                if (slot == null) return amount;
                amount = RemoveItem(slot, amount);
            }
            return amount;
        }

        public uint RemoveItem(InventorySlot slot, uint amount)
        {
            if (
                slot == null ||
                slot.Item == null)
                return amount;

            if(slot.OwnedAmount > amount)
            {
                slot.OwnedAmount -= amount;
                OnInventoryChanged?.Invoke(slot);
                return 0;
            }
            else
            {
                amount -= slot.OwnedAmount;
                if (!slot.KeepItem)
                {
                    slot.Item = null;
                }
                slot.OwnedAmount = 0;
                OnInventoryChanged?.Invoke(slot);
                return amount;
            }
        }

        public uint TotalInInventory(Item item)
        {
            uint counter = 0;
            foreach (InventorySlot slot in slots)
            {
                if (slot.Item == item) counter += slot.OwnedAmount;
            }
            return counter;
        }

        public InventorySlot HotbarChangeSelect(int value)
        {
            if (value == 0) return null;
            int newValue = hotbarSelectedIndex - value;
            while (newValue < 0) newValue = 10 + newValue;
            while (newValue > 9) newValue = newValue % 10;
            hotbarSelectedIndex = newValue;
            selectedSlot = slots[hotbarSelectedIndex];
            return selectedSlot;
        }

        public InventorySlot HotbarSetSelect(int index)
        {
            selectedSlot = slots[index];
            return selectedSlot;
        }

        public void SubscribeToOnSelectedItemChanged(Action<InventorySlot> listener)
        {
            OnSelectedSlotChanged += listener;
            listener?.Invoke(selectedSlot);
        }

        public void UnsubscribeToOnSelectedItemChanged(Action<InventorySlot> listener)
        {
            OnSelectedSlotChanged -= listener;
        }
    }
}