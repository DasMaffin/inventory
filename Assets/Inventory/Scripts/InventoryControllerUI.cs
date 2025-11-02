using Maffin.InvetorySystem.Inventories;
using Maffin.InvetorySystem.Slots;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControllerUI : MonoBehaviour
{
    public GameObject InventorySlotPrefab;
    public GameObject InventoryItemPrefab;
    public GameObject InventorySlotsArea;
    public GameObject InventoryHotbarArea;
    public GameObject ItemInfoArea;
    public Inventory inventory;
    public Dictionary<InventorySlot, GameObject> SlotToGO = new Dictionary<InventorySlot, GameObject>();
    public Dictionary<GameObject, InventorySlot> GOToSlot = new Dictionary<GameObject, InventorySlot>();

    private Dictionary<InventorySlot, InventoryItemController> InventoryItems = new Dictionary<InventorySlot, InventoryItemController>();

    private bool isInitialized = false;
    public void OpenInventory(Inventory _inventory)
    {
        InventorySlotsArea.SetActive(true);
        ItemInfoArea.SetActive(true);
        if (!isInitialized)
        {
            inventory = _inventory;
            
            _inventory.OnInventoryChanged += Inventory_OnInventoryChanged;

            for (int i = 0; i < inventory.slots.Length; i++)
            {
                Transform area;
                if (inventory.isLocalPlayerInventory && i < 10)
                    area = InventoryHotbarArea.transform;
                else
                    area = InventorySlotsArea.transform;

                SlotToGO.Add(inventory.slots[i], Instantiate(InventorySlotPrefab, area));
                GOToSlot.Add(SlotToGO[inventory.slots[i]], inventory.slots[i]);
                SlotToGO[inventory.slots[i]].GetComponent<InventorySlotController>().myInventory = this;
                if (inventory.slots[i].Item != null)
                    Inventory_OnInventoryChanged(inventory.slots[i]);
            }
            isInitialized = true;
        }
    }

    public void CloseInventory()
    {
        InventorySlotsArea.SetActive(false);
        ItemInfoArea.SetActive(false);

        ///
        /// This is the old code for opening/closing. I was convinced I still needed it for chest opening/closing as I thought I just regenerate them instead of caching them. 
        /// I changed it to caching for the player inventory, as it needs to always be active for the hotbar. Somehow it works PERFECT with chests as well.
        /// Keeping this code in case something does break so I remember what it used to be.
        ///

        //if (!InventorySlotsArea.activeSelf) return;
        //InventorySlotsArea.SetActive(false);
        //ItemInfoArea.SetActive(false);
        //inventory.OnInventoryChanged -= Inventory_OnInventoryChanged;
        //for (int i = 0; i < SlotToGO.Count; i++)
        //{
        //    Destroy(SlotToGO[inventory.slots[i]]);
        //}
        //SlotToGO.Clear();
        //GOToSlot.Clear();
        //InventoryItems.Clear();
    }

    private void Inventory_OnInventoryChanged(InventorySlot slot)
    {
        if (slot.Item != null)
        {
            if (InventoryItems.ContainsKey(slot) && InventoryItems[slot] == null)
            {
                InventoryItems.Remove(slot);
            }
            if (!InventoryItems.ContainsKey(slot))
            {
                // Spawning
                InventoryItemController iic = Instantiate(InventoryItemPrefab, SlotToGO[slot].transform).GetComponent<InventoryItemController>();
                InventoryItems.Add(slot, iic);
                InventoryItems[slot].MyItem = slot.Item;
            }
        }

        if (slot.OwnedAmount == 0)
        {
            Destroy(InventoryItems[slot].gameObject);
            InventoryItems.Remove(slot);
            return;
        }
        InventoryItems[slot].SetAmount(slot.OwnedAmount);
    }
}