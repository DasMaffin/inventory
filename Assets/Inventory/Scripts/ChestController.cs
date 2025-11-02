using Maffin.InvetorySystem.Inventories;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChestController : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI text;
    [HideInInspector] public Inventory Inventory;
    [HideInInspector] public InventoryControllerUI InventoryUI;
    [HideInInspector] public GameObject InventoryCanvas;
    [HideInInspector] public string chestText
    {
        set
        {
            text.text = value;
        }
    }

    private static List<ChestController> allChests = new List<ChestController>();

    private void Start()
    {
        InventoryUI.InventoryHotbarArea.SetActive(false);
        allChests.Add(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleInventory();
    }

    private bool inventoryOpen = false;
    public void ToggleInventory()
    {
        foreach (var item in allChests)
        {
            if(item != this)
            {
                item.closeInventory();
            }
        }

        inventoryOpen = !inventoryOpen;
        if (inventoryOpen)
        {
            InventoryUI.OpenInventory(Inventory);
        }
        else
        {
            InventoryUI.CloseInventory();
        }
        InventoryCanvas.SetActive(inventoryOpen);
    }

    private void closeInventory()
    {
        InventoryUI.CloseInventory();
        inventoryOpen = false;
    }
}
