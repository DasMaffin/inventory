using Maffin.InvetorySystem.Inventories;
using Maffin.InvetorySystem.Slots;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InventoryItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI AmountText;

    private RectTransform rectTransform;
    private Canvas rootCanvas;
    private RectTransform canvasRect;
    private Camera canvasCamera;
    private GraphicRaycaster raycaster;
    private InventoryControllerUI inventoryControllerUI;

    private Transform originalParent;

    private bool isDragged = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        inventoryControllerUI = GetComponentInParent<InventoryControllerUI>();
        rootCanvas = GetComponentInParent<Canvas>();
        if (rootCanvas == null)
            Debug.LogError("InventoryItemController must be a child of a Canvas.");
        else
        {
            canvasRect = rootCanvas.transform as RectTransform;
            canvasCamera = (rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : rootCanvas.worldCamera; 
            raycaster = rootCanvas.GetComponent<GraphicRaycaster>();
        }
    }

    public void SetAmount(uint amount)
    {
        if (AmountText != null)
            AmountText.text = amount.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (rootCanvas == null) return;

        isDragged = true;

        // Store original parent so we can return the item later
        originalParent = transform.parent;

        // Reparent to canvas so the item is not affected by parent's anchors/pivots/LayoutGroups
        transform.SetParent(rootCanvas.transform, true); // worldPositionStays = true, so visible position doesn't jump
        transform.SetAsLastSibling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragged) return;

        isDragged = false;


        GameObject hoveredSlotGO = GetUIUnderMouseWithTag("InventorySlot");
        if (hoveredSlotGO != null && hoveredSlotGO.transform != originalParent)
        {
            InventorySlot hoveredSlot = inventoryControllerUI.GOToSlot[hoveredSlotGO];
            InventorySlot originalSlot = inventoryControllerUI.GOToSlot[originalParent.gameObject];
            uint ret = inventoryControllerUI.inventory.AddItem(hoveredSlot, originalSlot.Item, originalSlot.OwnedAmount);
            inventoryControllerUI.inventory.RemoveItem(originalSlot, originalSlot.MaxAmount - ret);
            if (ret != 0) resetToOriginal();
        }
        else
        {
            resetToOriginal();
        }
    }

    private void resetToOriginal()
    {
        transform.SetParent(originalParent, false);
        rectTransform.anchorMin = new Vector2(0.1f, 0.1f);
        rectTransform.anchorMax = new Vector2(0.9f, 0.9f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
    }

    private void Update()
    {
        if (!isDragged || rootCanvas == null) return;

        Vector2 mousePos = Mouse.current != null ? Mouse.current.position.ReadValue() : (Vector2)Input.mousePosition;

        mousePos.x = Mathf.Clamp(mousePos.x, 0f, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0f, Screen.height);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePos,
            canvasCamera,
            out localPoint
        );

        rectTransform.anchoredPosition = localPoint;
    }

    private GameObject GetUIUnderMouseWithTag(string tag)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(tag))
                return result.gameObject;
        }

        return null;
    }
}
