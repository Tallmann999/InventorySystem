using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlotUi : MonoBehaviour
{
    [SerializeField] private EquipmentSlotType _slotType;
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _quantityText;

    private Outline _outline;
    private ItemSlot _currentSlot;
    //private EquipmentManager _equipmentManager;
    private bool _isEquipped;

    public EquipmentSlotType SlotType => _slotType;

    private void OnEnable()
    {        
        _button.onClick.AddListener(OnSlotClick);
    }

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _button = GetComponent<Button>();
        ClearSlot();
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnSlotClick);
    }

    public void SetSlot(ItemSlot slot)
    {
        _currentSlot = slot;

        if (slot != null && slot.item != null)
        {
            _icon.gameObject.SetActive(true);
            _icon.sprite = slot.item.Icon;
            _isEquipped = true;
        }
        else
        {
            _icon.gameObject.SetActive(false);
            _isEquipped = false;
        }
        UpdateVisualOutline();
    }

    private void UpdateVisualOutline()
    {
        if (_outline != null)
        {
            _outline.enabled = _isEquipped;
        }
    }

    public void ClearSlot()
    {
        _currentSlot = null;
        _icon.gameObject.SetActive(false);
        _isEquipped = false;
        _quantityText.text = string.Empty;

        UpdateVisualOutline();
    }

    public void OnSlotClick()
    {
        // При клике на слот экипировки - выделяем его
        if (_currentSlot != null && _currentSlot.item != null)
        {
            Inventory.Instanse.SelectEquipmentItem(_currentSlot);
        }
    }
}