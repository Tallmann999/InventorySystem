using System;
using UnityEngine;
using static EquipmentManager;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private ItemSlotUi[] _equipedSlotsUi;
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
            _equipedSlotsUi[i].index = 1;
            _equipedSlotsUi[i].ClearSlot();
        }
    }

    //public bool EquipItem(ItemDataBase item)
    //{
    //    for (int i = 0; i < _equipedSlots.Length; i++)
    //    {
    //        if (_equipedSlots[i].Type == item.Type.)
    //        {

    //        }
    //    }
    //}

    public bool IsItemEquipped(ItemDataBase item)
    {
        throw new NotImplementedException();
    }

    public bool UnequipItem(ItemDataBase item)
    {
        throw new NotImplementedException();
    }
}