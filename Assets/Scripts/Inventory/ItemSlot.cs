using UnityEngine;

public class ItemSlot
{
    public ItemDataBase item;
    public EquipmentSlotType Type;
    public int Quantity;
    public bool IsEquipped;

    public ItemSlot()
    {
        item = null;
        Quantity = 0;
        IsEquipped = false;
    }

    // Конструктор для обратной совместимости
    public ItemSlot(EquipmentSlotType type)
    {
        item = null;
        Quantity = 0;
        Type = type;
    }

}
