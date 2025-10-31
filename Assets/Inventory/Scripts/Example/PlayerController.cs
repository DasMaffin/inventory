using Maffin.InvetorySystem.Builders;
using Maffin.InvetorySystem.Inventories;
using Maffin.InvetorySystem.Items;
using Maffin.InvetorySystem.Slots;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
    public Inventory PlayerInventory;

    [SerializeField] private PlayerInput PlayerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Inventory.SetItemsPath("Examples/");
        PlayerInventory = InventoryBuilder.Create()
            .SetOwner(this.gameObject)
            .SetCapacity(20)
            .SetLocalPlayer()
            .Build();
    }

    private void OnEnable()
    {
        PlayerInput.actions["ScrollWheel"].performed += OnScrollWheel;

        PlayerInventory.SubscribeToOnSelectedItemChanged(OnSelectedItemChanged);
    }

    private void OnSelectedItemChanged(InventorySlot slot)
    {
        if (slot.Item == null || slot.OwnedAmount == 0) return;
        print(slot.Item.itemName);
    }

    private void OnScrollWheel(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<Vector2>().y;

        PlayerInventory.HotbarChangeSelect((int)scrollValue * -1);
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
