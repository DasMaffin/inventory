using Maffin.InvetorySystem.Slots;
using UnityEngine;

public class InventorySlotController : MonoBehaviour
{
    public GameObject SelectedOutline;
    public InventoryControllerUI myInventory;

    private void Awake()
    {
        Select(false);
    }

    private void Start()
    {
        myInventory.inventory.SubscribeToOnSelectedItemChanged(OnSelecedItemChanged);
    }

    private void OnDisable()
    {
        myInventory.inventory.UnsubscribeToOnSelectedItemChanged(OnSelecedItemChanged);
    }

    private void OnSelecedItemChanged(InventorySlot slot)
    {
        if (slot == null) return;
        Select(myInventory.GOToSlot[gameObject] == slot);
    }

    private void Select(bool value)
    {
        SelectedOutline.SetActive(value);
    }
}
