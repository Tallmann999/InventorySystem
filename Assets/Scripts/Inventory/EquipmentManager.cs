using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private EquipmentSlotUi[] _equipedSlotsUi;
    [SerializeField] private ItemSlot[] _equipedSlots;

    private void Start()
    {
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        _equipedSlots = new ItemSlot[_equipedSlotsUi.Length];

        for (int i = 0; i < _equipedSlots.Length; i++)
        {
            _equipedSlots[i] = new ItemSlot();
            _equipedSlotsUi[i].SetSlot(_equipedSlots[i]);
        }
    }

    public void EquipItem(ItemSlot selectItem)
    {
        if (selectItem == null || selectItem.item == null)
        {
            Debug.LogError("������� ����������� ������ ����!");
            return;
        }

        ItemSlot equipmentSlot = GetEquipmentSlotForItem(selectItem);

        if (equipmentSlot != null)
        {
            // ��������� ������ ������� (���� ����)
            ItemDataBase oldItem = equipmentSlot.item;

            // ��������� ����� �������
            equipmentSlot.item = selectItem.item;
            equipmentSlot.Quantity = 1;
            equipmentSlot.IsEquipped = true;

            // ������� ���� � ���������
            selectItem.item = null;
            selectItem.Quantity = 0;
            selectItem.IsEquipped = false;

            // ���������� ������ ������� � ��������� (���� ���)
            if (oldItem != null)
            {
                Inventory.Instanse.AddItem(oldItem);
                ApplyEquipmentStats(oldItem.EquipmentItems[0], false);
            }

            // ��������� ����� ������ ��������
            ApplyEquipmentStats(equipmentSlot.item.EquipmentItems[0], true);

            // ��������� UI
            UpdateEquipmentUi();
            Inventory.Instanse.UpdateUi();

            Debug.Log($"����������: {equipmentSlot.item.ItemName}");
        }
        else
        {
            Debug.LogError($"�� ������ ���������� ���� ��� ��������: {selectItem.item.ItemName}");
        }
    }

    public bool UnequipItem(ItemDataBase item)
    {
        if (item == null || item.EquipmentItems == null || item.EquipmentItems.Length == 0)
            return false;

        for (int i = 0; i < _equipedSlots.Length; i++)
        {
            if (_equipedSlots[i].item == item)
            {
                // ������� ���� ����������
                _equipedSlots[i].item = null;
                _equipedSlots[i].Quantity = 0;
                _equipedSlots[i].IsEquipped = false;

                // ������� �����
                ApplyEquipmentStats(item.EquipmentItems[0], false);

                // ��������� UI ����������
                UpdateEquipmentUi();

                // ��������� ������� ������� � ���������
                Inventory.Instanse.AddItem(item);

                Debug.Log($"���� � ��������� � ���������: {item.ItemName}");
                return true;
            }
        }

        Debug.LogError($"������� �� ������ � ����������: {item.ItemName}");
        return false;
    }
    public bool DropEquippedItem(ItemDataBase item)
    {
        if (item == null || item.EquipmentItems == null || item.EquipmentItems.Length == 0)
            return false;

        for (int i = 0; i < _equipedSlots.Length; i++)
        {
            if (_equipedSlots[i].item == item)
            {
                // ������� ���� ����������
                _equipedSlots[i].item = null;
                _equipedSlots[i].Quantity = 0;
                _equipedSlots[i].IsEquipped = false;

                // ������� �����
                ApplyEquipmentStats(item.EquipmentItems[0], false);

                // ��������� UI
                UpdateEquipmentUi();

                Debug.Log($"��������� �� ����������: {item.ItemName}");
                return true;
            }
        }
        return false;
    }

    public bool IsItemEquipped(ItemDataBase item)
    {
        if (item == null) return false;

        foreach (var slot in _equipedSlots)
        {
            if (slot.item == item && slot.IsEquipped)
                return true;
        }
        return false;
    }

    private ItemSlot GetEquipmentSlotForItem(ItemSlot selectItem)
    {
        if (selectItem?.item?.EquipmentItems?.Length > 0 &&
            selectItem.item.Type == ItemType.Equipped)
        {
            EquipmentSlotType requiredType = selectItem.item.EquipmentItems[0].EquipmentSlot;

            for (int i = 0; i < _equipedSlots.Length; i++)
            {
                if (_equipedSlotsUi[i].SlotType == requiredType)
                    return _equipedSlots[i];
            }
        }
        return null;
    }

    private void ApplyEquipmentStats(EquipmentItem equipmentData, bool add)
    {
        if (equipmentData == null) return;

        int multiplier = add ? 1 : -1;
        Debug.Log($"{(add ? "���������" : "�����")} �����: ����� {equipmentData.Attack * multiplier}, ������ {equipmentData.Defense * multiplier}");
    }

    public void UpdateEquipmentUi()
    {
        for (int i = 0; i < _equipedSlots.Length; i++)
        {
            if (_equipedSlots[i].item != null)
            {
                _equipedSlotsUi[i].SetSlot(_equipedSlots[i]);
            }
            else
            {
                _equipedSlotsUi[i].ClearSlot();
            }
        }
    }

    // ����� ��� ��������� �������� �� ����� ����������
    public ItemSlot GetEquipmentSlot(EquipmentSlotType slotType)
    {
        for (int i = 0; i < _equipedSlotsUi.Length; i++)
        {
            if (_equipedSlotsUi[i].SlotType == slotType)
                return _equipedSlots[i];
        }
        return null;
    }
}