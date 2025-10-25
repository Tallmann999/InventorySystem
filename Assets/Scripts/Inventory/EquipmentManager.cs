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
            Debug.LogError("Попытка экипировать пустой слот!");
            return;
        }

        ItemSlot equipmentSlot = GetEquipmentSlotForItem(selectItem);

        if (equipmentSlot != null)
        {
            // Сохраняем старый предмет (если есть)
            ItemDataBase oldItem = equipmentSlot.item;

            // Экипируем новый предмет
            equipmentSlot.item = selectItem.item;
            equipmentSlot.Quantity = 1;
            equipmentSlot.IsEquipped = true;

            // Очищаем слот в инвентаре
            selectItem.item = null;
            selectItem.Quantity = 0;
            selectItem.IsEquipped = false;

            // Возвращаем старый предмет в инвентарь (если был)
            if (oldItem != null)
            {
                Inventory.Instanse.AddItem(oldItem);
                ApplyEquipmentStats(oldItem.EquipmentItems[0], false);
            }

            // Применяем статы нового предмета
            ApplyEquipmentStats(equipmentSlot.item.EquipmentItems[0], true);

            // Обновляем UI
            UpdateEquipmentUi();
            Inventory.Instanse.UpdateUi();

            Debug.Log($"Экипирован: {equipmentSlot.item.ItemName}");
        }
        else
        {
            Debug.LogError($"Не найден подходящий слот для предмета: {selectItem.item.ItemName}");
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
                // Очищаем слот экипировки
                _equipedSlots[i].item = null;
                _equipedSlots[i].Quantity = 0;
                _equipedSlots[i].IsEquipped = false;

                // Снимаем статы
                ApplyEquipmentStats(item.EquipmentItems[0], false);

                // Обновляем UI экипировки
                UpdateEquipmentUi();

                // ДОБАВЛЯЕМ предмет обратно в инвентарь
                Inventory.Instanse.AddItem(item);

                Debug.Log($"Снят и возвращен в инвентарь: {item.ItemName}");
                return true;
            }
        }

        Debug.LogError($"Предмет не найден в экипировке: {item.ItemName}");
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
                // Очищаем слот экипировки
                _equipedSlots[i].item = null;
                _equipedSlots[i].Quantity = 0;
                _equipedSlots[i].IsEquipped = false;

                // Снимаем статы
                ApplyEquipmentStats(item.EquipmentItems[0], false);

                // Обновляем UI
                UpdateEquipmentUi();

                Debug.Log($"Выброшено из экипировки: {item.ItemName}");
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
        Debug.Log($"{(add ? "Применены" : "Сняты")} статы: Атака {equipmentData.Attack * multiplier}, Защита {equipmentData.Defense * multiplier}");
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

    // Метод для получения предмета из слота экипировки
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