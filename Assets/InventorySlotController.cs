using Maffin.InvetorySystem.Slots;
using System;
using UnityEngine;

public class InventorySlotController : MonoBehaviour
{
    public GameObject SelectedOutline;

    private void Awake()
    {
        Select(false);
    }

    private void Start()
    {
        InventoryControllerUI.Instance.inventory.SubscribeToOnSelectedItemChanged(OnSelecedItemChanged);
    }

    private void OnDisable()
    {
        InventoryControllerUI.Instance.inventory.UnsubscribeToOnSelectedItemChanged(OnSelecedItemChanged);
    }

    private void OnSelecedItemChanged(InventorySlot slot)
    {
        Select(InventoryControllerUI.Instance.GOToSlot[gameObject] == slot);
    }

    private void Select(bool value)
    {
        SelectedOutline.SetActive(value);
    }
}
