using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private Image highlightedBGImage;
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button button;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);

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
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }
}
