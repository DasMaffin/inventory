using Maffin.InvetorySystem.Builders;
using Maffin.InvetorySystem.Inventories;
using Maffin.InvetorySystem.Items;
using Maffin.InvetorySystem.Slots;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
    [HideInInspector] public Inventory PlayerInventory;
    [HideInInspector] public InventoryControllerUI InventoryUI;

    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private GameObject InventoryPrefab;
    [SerializeField] private GameObject InventoryCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Inventory.SetItemsPath("Examples/");
        PlayerInventory = InventoryBuilder.Create()
            .SetOwner(this.gameObject)
            .SetCapacity(40)
            .SetLocalPlayer()
            .Build();

        InventoryUI = Instantiate(InventoryPrefab, InventoryCanvas.transform).GetComponent<InventoryControllerUI>();
    }

    private void OnEnable()
    {
        PlayerInput.actions["ScrollWheel"].performed += OnScrollWheel;

        PlayerInventory.SubscribeToOnSelectedItemChanged(OnSelectedItemChanged);
        InventoryUI.OpenInventory(PlayerInventory);
        InventoryUI.CloseInventory();
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
            InventoryUI.CloseInventory();
        }
        else
        {
            InventoryUI.OpenInventory(PlayerInventory);
        }

        inventoryOpen = !inventoryOpen;
    }
}
