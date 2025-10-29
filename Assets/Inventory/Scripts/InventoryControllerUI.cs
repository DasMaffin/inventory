using Maffin.InvetorySystem.Inventories;
using Maffin.InvetorySystem.Slots;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryControllerUI : MonoBehaviour
{
    private static InventoryControllerUI instance;
    public static InventoryControllerUI Instance
    {
        get => instance;
        set
        {
            instance = value;
        }
    }

    public GameObject InventorySlotPrefab;
    public GameObject InventoryItemPrefab;
    public GameObject InventorySlotsArea;
    public Inventory inventory;
    public Dictionary<InventorySlot, GameObject> SlotToGO = new Dictionary<InventorySlot, GameObject>();
    public Dictionary<GameObject, InventorySlot> GOToSlot = new Dictionary<GameObject, InventorySlot>();

    private Dictionary<InventorySlot, InventoryItemController> InventoryItems = new Dictionary<InventorySlot, InventoryItemController>();

    private void Awake()
    {
        Instance = this;
    }

    public void OpenInventory(Inventory _inventory)
    {
        inventory = _inventory;
        InventorySlotsArea.SetActive(true);
        _inventory.OnInventoryChanged += Inventory_OnInventoryChanged;

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            SlotToGO.Add(inventory.slots[i], Instantiate(InventorySlotPrefab, InventorySlotsArea.transform));
            GOToSlot.Add(SlotToGO[inventory.slots[i]], inventory.slots[i]);
            if (inventory.slots[i].Item != null) 
                Inventory_OnInventoryChanged(inventory.slots[i]);
        }
    }

    public void CloseInventory()
    {
        InventorySlotsArea.SetActive(false);
        inventory.OnInventoryChanged -= Inventory_OnInventoryChanged;
        for (int i = 0; i < SlotToGO.Count; i++)
        {
            Destroy(SlotToGO[inventory.slots[i]]);
        }
        SlotToGO.Clear();
        GOToSlot.Clear();
        InventoryItems.Clear();
    }

    private void Inventory_OnInventoryChanged(InventorySlot slot)
    {
        if(!InventoryItems.ContainsKey(slot) && slot.Item != null)
        {
            InventoryItems.Add(slot, Instantiate(InventoryItemPrefab, SlotToGO[slot].transform).GetComponent<InventoryItemController>());
            InventoryItems[slot].GetComponent<Image>().sprite = slot.Item.Icon;
        }
        else if(slot.Item == null)
        {
            Destroy(InventoryItems[slot].gameObject);
            InventoryItems.Remove(slot);
            return;
        }
        InventoryItems[slot].SetAmount(slot.OwnedAmount);
    }
}
