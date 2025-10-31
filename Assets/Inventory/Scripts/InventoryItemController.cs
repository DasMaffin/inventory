using Maffin.InvetorySystem.Inventories;
using Maffin.InvetorySystem.Items;
using Maffin.InvetorySystem.Slots;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InventoryItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public TextMeshProUGUI AmountText;
    private Item myItem; // If I ever need the slot just replace this with it as the slot knows the item.
    public Item MyItem
    {
        get => myItem;
        set
        {
            myItem = value;
            MyImage.sprite = MyItem.Icon;
        }
    }
    private bool isPlaceholder;
    private bool IsPlaceholder
    {
        get => isPlaceholder;
        set
        {
            isPlaceholder = value;
            MyImage.color = new Color(1f, 1f, 1f, 0.25f);
        }
    }
    private RectTransform rectTransform;
    private Canvas rootCanvas;
    private RectTransform canvasRect;
    private Camera canvasCamera;
    private GraphicRaycaster raycaster;
    private InventoryControllerUI inventoryControllerUI;

    private Image myImage;
    private Image MyImage
    {
        get
        {
            if(myImage == null)
            {
                myImage = GetComponent<Image>();
            }
            return myImage;
        }
    }

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

    private void Start()
    {
        if (IsPlaceholder)
        {
            transform.SetAsFirstSibling();
        }
        else
        {
            transform.SetAsLastSibling();
        }
    }

    public void SetAmount(uint amount)
    {
        if (AmountText != null)
            AmountText.text = amount.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (rootCanvas == null || 
            eventData.button == PointerEventData.InputButton.Middle ||
            inventoryControllerUI.GOToSlot[transform.parent.gameObject].OwnedAmount == 0) return;

        isDragged = true;

        originalParent = transform.parent;

        transform.SetParent(rootCanvas.transform, true);
        transform.SetAsLastSibling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragged || eventData.button == PointerEventData.InputButton.Middle) return;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            InventorySlot slot = inventoryControllerUI.GOToSlot[transform.parent.gameObject];
            slot.KeepItem = !slot.KeepItem;

            if (!IsPlaceholder && slot.KeepItem)
            {
                slot.placeholder = Instantiate(InventoryControllerUI.Instance.InventoryItemPrefab, transform.parent.transform);
                InventoryItemController iic = slot.placeholder.GetComponent<InventoryItemController>();
                iic.MyItem = this.MyItem;
                iic.IsPlaceholder = true;
            }
            else
            {
                Destroy(slot.placeholder.gameObject);
            }
        }
    }
}
