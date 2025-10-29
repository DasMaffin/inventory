using Maffin.InvetorySystem.Builders;
using Maffin.InvetorySystem.Inventories;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
    public Inventory PlayerInventory;

    private PlayerInput PlayerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Inventory.SetItemsPath("Examples/");
        PlayerInventory = InventoryBuilder.Create()
            .SetOwner(this.gameObject)
            .SetCapacity(20)
            .Build();
    }

    private bool inventoryOpen = false;
    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;

        if (inventoryOpen)
        {
            InventoryControllerUI.Instance.CloseInventory();
        }
        else
        {
            InventoryControllerUI.Instance.OpenInventory(PlayerInventory);
        }

        inventoryOpen = !inventoryOpen;
    }
}
