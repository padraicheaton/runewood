using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventorySlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private Image highlightedBGImage;
    [SerializeField] private InventorySlot assignedInventorySlot;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    public UnityAction OnItemDataChanged;

    private void Awake()
    {
        ClearSlot();

        ParentDisplay = FindParentDisplay();

        SetHighlighted(false);
    }

    private InventoryDisplay FindParentDisplay()
    {
        InventoryDisplay parentDisplay = null;
        Transform parent = transform.parent;

        while (parentDisplay == null)
        {
            parentDisplay = parent.GetComponent<InventoryDisplay>();

            if (parentDisplay == null)
                parent = parent.parent;

            if (parent == null)
                break;
        }

        return parentDisplay;
    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;

        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.Data != null)
        {
            itemIcon.sprite = slot.Data.icon;
            itemIcon.color = Color.white;

            if (slot.StackSize > 1)
                itemCount.text = slot.StackSize.ToString();
            else
                itemCount.text = "";

            OnItemDataChanged?.Invoke();
        }
        else
            ClearSlot();
    }

    public void SetHighlighted(bool highlighted)
    {
        highlightedBGImage.color = highlighted ? Color.white : Color.clear;
    }

    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null)
            UpdateUISlot(assignedInventorySlot);
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();

        itemIcon.sprite = null;
        itemIcon.color = Color.clear;
        itemCount.text = "";

        OnItemDataChanged?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseItemData.Instance.HoveredOverItemSlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseItemData.Instance.ExitHoveredItemSlot(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ParentDisplay?.SlotClicked(this, eventData.button.Equals(PointerEventData.InputButton.Right));
    }
}
