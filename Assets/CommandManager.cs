using Maffin.InvetorySystem.Inventories;
using System;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public PlayerController PlayerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CommandCenter.AddCommand("AddItem", (value) =>
        {
            int index = Convert.ToInt16(value[0]);
            if (index > Inventory.AllItems.Count)
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
            int index = Convert.ToInt16(value[0]);
            if (index > Inventory.AllItems.Count)
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
    }
}
