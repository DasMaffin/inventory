using Maffin.InvetorySystem.Builders;
using Maffin.InvetorySystem.Inventories;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public Transform ChestList;
    public Dictionary<GameObject, InventoryControllerUI> GoToInventoryUI = new Dictionary<GameObject, InventoryControllerUI>();

    [SerializeField] private GameObject InventoryPrefab;
    [SerializeField] private GameObject InventoryCanvas;
    [SerializeField] private GameObject ChestPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InventoryCanvas.SetActive(false);
        CommandCenter.AddCommand("AddItem", (value) =>
        {
            bool success = int.TryParse(value[0], out int index);
            if (!success || index > Inventory.AllItems.Count)
            {
                Debug.LogWarning($"There are only {Inventory.AllItems.Count} Items! Usage: AddItem {{ItemIndex}} {{Amount}}.\nIndex ranges from 0 to {Inventory.AllItems.Count - 1}");
                return;
            }
            uint amount = 1;

            if (value.Length > 1 && uint.TryParse(value[1], out uint parsed))
            {
                amount = parsed;
            }
            PlayerController.PlayerInventory.AddItem(Inventory.AllItems[index], amount);
        });
        CommandCenter.AddCommand("RemoveItem", (value) =>
        {
            bool success = int.TryParse(value[0], out int index);
            if (!success || index > Inventory.AllItems.Count)
            {
                Debug.LogWarning($"There are only {Inventory.AllItems.Count} Items! Usage: AddItem {{ItemIndex}} {{Amount}}.\nIndex ranges from 0 to {Inventory.AllItems.Count - 1}");
                return;
            }
            uint amount = 1;

            if (value.Length > 1 && uint.TryParse(value[1], out uint parsed))
            {
                amount = parsed;
            }
            PlayerController.PlayerInventory.RemoveItem(Inventory.AllItems[index], amount);
        });
        CommandCenter.AddCommand("AddChest", (value) =>
        {
            if (value.Length < 1) return;
            bool success = uint.TryParse(value[0], out uint slots);
            if (!success || slots < 10)
            {
                Debug.LogWarning($"You need to specify how many slots there are, with a minimum of 10!");
                return;
            }
            GameObject owner = Instantiate(ChestPrefab, ChestList);
            ChestController chest = owner.GetComponent<ChestController>();
            chest.Inventory = InventoryBuilder.Create()
            .SetCapacity(slots)
            .Build();
            

            GoToInventoryUI[owner] = Instantiate(InventoryPrefab, InventoryCanvas.transform).GetComponent<InventoryControllerUI>();
            chest.InventoryUI = GoToInventoryUI[owner];
            chest.InventoryCanvas = InventoryCanvas;
            chest.chestText = $"Chest with {slots} slots.";
        });
    }
}
