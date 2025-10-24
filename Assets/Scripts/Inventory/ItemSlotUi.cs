using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUi : MonoBehaviour
{
    [SerializeField] private EquipmentSlotType _slotType;// ������ ����� ����� ��� ����� ��� ������� � itemDatabase
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private Image _background; // ��� ����������� ������� ����� ������

    private Outline _outline;
    private ItemSlot _currentSlot;
    public int index;
    public bool _isEquipped;

    //public int Index => _index;
    public EquipmentSlotType SlotType => _slotType;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        UpdateVisualOutline();
    }

    public void SetSlot(ItemSlot slot)
    {
        _currentSlot = slot;

        if (slot.item != null)
        {
            _icon.gameObject.SetActive(true);
            _icon.sprite = slot.item.Icon;
            _quantityText.text = slot.Quantity > 1 ? slot.Quantity.ToString() : string.Empty;
        }
        else
        {
            _icon.gameObject.SetActive(false);
            _quantityText.text = string.Empty;
        }

        UpdateVisualOutline();

    }

    private void UpdateVisualOutline()
    {
        if (_outline != null)
        {
            // ������������ ���� ������� ����������
            _outline.enabled = _currentSlot != null && _currentSlot.IsEquipped;
        }
    }

    public void ClearSlot()
    {
        _currentSlot = null;
        _icon.gameObject.SetActive(false);
        _quantityText.text = string.Empty;
        UpdateVisualOutline();
    }

    public void OnClickButton()
    {
        Inventory.Instanse.SelectItem(index);
    }

    //public  void  SetIndex(int index)
    //{       
    //    index = index;
    //}
}
