using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(EquipmentManager))]
public class Inventory : MonoBehaviour
{
    public ItemSlotUi[] uiSlots;
    public ItemSlot[] slots;

    [SerializeField] private GameObject _inventoryWindow;
    [SerializeField] private Transform _dropPosition;
    [SerializeField] private EquipmentManager _equipmentManager;

    [Header("Выбор предмета")]
    [SerializeField] private TextMeshProUGUI _selectedItemName;
    [SerializeField] private TextMeshProUGUI _selectedItemDiscription;
    [SerializeField] private TextMeshProUGUI _selectedItemStatsName;
    [SerializeField] private TextMeshProUGUI _selectedItemStatsNameValue;

    [SerializeField] private GameObject _useButton;
    [SerializeField] private GameObject _equipButton;
    [SerializeField] private GameObject _unequipButton;
    [SerializeField] private GameObject _dropButton;

    private PlayerNeeds _playerNeeds;
    private ItemSlot _selectItem;
    private int _selectItemIndex;
    private int _currentEquipIndex;

    private GameObject _playerController;// Здесь должно быть обращение к PlayerConrtoller

    [Header("События")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    public static Inventory Instanse;

    private void Awake()
    {
        Instanse = this;
        _playerNeeds = GetComponent<PlayerNeeds>();
        _equipmentManager = GetComponent<EquipmentManager>();
        //_playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _inventoryWindow.SetActive(false);

      
        InitializeSlots();
        ClearSelectedItemWindow();
    }

    private void InitializeSlots()
    {
        slots = new ItemSlot[uiSlots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot(EquipmentSlotType.Default);
            uiSlots[i].index = i;
            uiSlots[i].ClearSlot();
        }
    }


    public void OnInventoryButton()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (_inventoryWindow.activeInHierarchy)
        {
            _inventoryWindow.SetActive(false);
            onCloseInventory.Invoke();
            ClearSelectedItemWindow();
        }
        else
        {
            _inventoryWindow.SetActive(true);
            onOpenInventory.Invoke();
        }
    }

    public bool IsOpen()
    {
        return _inventoryWindow.activeInHierarchy;
    }

    public void AddItem(ItemDataBase item)
    {
        if (item.canStackable)
        {

            ItemSlot slotTostack = GetItemStack(item);

            if (slotTostack != null)
            {
                slotTostack.Quantity++;
                UpdateUi();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.Quantity = 1;
            UpdateUi();
            return;
        }

        ThrowItem(item);
    }

    public void ThrowItem(ItemDataBase item)
    {
        if (item == null)
        {
            Debug.LogError("Попытка выбросить null предмет!");
            return;
        }

        if (item.DropPrefab == null)
        {
            Debug.LogError($"У предмета {item.ItemName} не назначен DropPrefab!");
            return;
        }

        Instantiate(item.DropPrefab, _dropPosition.position, Quaternion.identity);
        Debug.Log($"Выброшен предмет: {item.ItemName}");
    }

    public void UpdateUi()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];

            if (slot != null && slot.item != null)
            {
                Debug.Log("Предмет должен появиться: " + slot.item.ItemName);
                uiSlots[i].SetSlot(slot);
            }
            else
            {
                uiSlots[i].ClearSlot();
            }
        }
    }

    private ItemSlot GetItemStack(ItemDataBase item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].Quantity < item.MaxStack)
            {
                return slots[i];
            }
        }
        return null;
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void SelectItem(int index)// тот слот который мы нажимаем в инвентаре
    {
        if (slots[index].item == null)
            return;

        _selectItem = slots[index];
        _selectItemIndex = index;
        _selectedItemName.text = _selectItem.item.ItemName;
        _selectedItemDiscription.text = _selectItem.item.Discription;
        _selectedItemStatsName.text = string.Empty;
        _selectedItemStatsNameValue.text = string.Empty;

        for (int i = 0; i < _selectItem.item.ConsumableEffects.Length; i++)
        {// сделать это через Свитч
            _selectedItemStatsName.text += _selectItem.item.ConsumableEffects[i].ConsumableType.ToString() + "\n";
            _selectedItemStatsNameValue.text += _selectItem.item.ConsumableEffects[i].Value.ToString() + "\n";
        }


        for (int i = 0; i < _selectItem.item.EquipmentItems.Length; i++)
        {// сделать это через Свитч
            _selectedItemStatsName.text += "Атака" + "\n";
            _selectedItemStatsName.text += "Защита" + "\n";
            _selectedItemStatsName.text += "Скорость Атаки" + "\n";
            _selectedItemStatsNameValue.text += _selectItem.item.EquipmentItems[i].Attack.ToString() + "\n";
            _selectedItemStatsNameValue.text += _selectItem.item.EquipmentItems[i].Defense.ToString() + "\n";
            _selectedItemStatsNameValue.text += _selectItem.item.EquipmentItems[i].AttackSpeed.ToString() + "\n";
        }


        _useButton.SetActive(_selectItem.item.Type == ItemType.Consumed);
        _equipButton.SetActive(_selectItem.item.Type == ItemType.Equipped && !uiSlots[index]._isEquipped);
        _unequipButton.SetActive(_selectItem.item.Type == ItemType.Equipped && uiSlots[index]._isEquipped);

        _dropButton.SetActive(true);

    }

    public void ClearSelectedItemWindow()
    {
        _selectItem = null;
        _selectedItemName.text = string.Empty;
        _selectedItemDiscription.text = string.Empty;
        _selectedItemStatsName.text = string.Empty;
        _selectedItemStatsNameValue.text = string.Empty;

        _useButton.SetActive(false);
        _equipButton.SetActive(false);
        _unequipButton.SetActive(false);
        _dropButton.SetActive(false);
    }

    public void OnUseButton()
    {
        if (_selectItem.item.Type == ItemType.Consumed)
        {
            for (int i = 0; i < _selectItem.item.ConsumableEffects.Length; i++)
            {
                switch (_selectItem.item.ConsumableEffects[i].ConsumableType)
                {
                    case ConsumableType.Health:
                        _playerNeeds.Heal(_selectItem.item.ConsumableEffects[i].Value);
                        break;
                    case ConsumableType.Hunger:
                        _playerNeeds.Eat(_selectItem.item.ConsumableEffects[i].Value);
                        break;
                    case ConsumableType.Thirst:
                        _playerNeeds.Drink(_selectItem.item.ConsumableEffects[i].Value);
                        break;
                    case ConsumableType.Energy:
                        _playerNeeds.DrinkEnergy(_selectItem.item.ConsumableEffects[i].Value);
                        break;

                    default:
                        break;
                }
            }
        }
        RemoveSelectedItem();
    }

    public void OnUnEquipButton()
    {
        if (_selectItem == null || _selectItem.item == null || _selectItem.item.Type != ItemType.Equipped)
            return;

        // Сохраняем информацию о предмете перед снятием
        string itemName = _selectItem.item.ItemName;

        // Пытаемся снять предмет через EquipmentManager
        if (_equipmentManager.UnequipItem(_selectItem.item))
        {
            // Если успешно сняли, очищаем выделение
            ClearSelectedItemWindow();
            UpdateUi(); // Обновляем UI инвентаря, чтобы показать возвращенный предмет
            Debug.Log($"Предмет {itemName} снят и возвращен в инвентарь");
        }
        else
        {
            Debug.LogError("Не удалось снять предмет!");
        }
    }

    private void UnEquipItem(int index)
    {

    }

    public void OnEquipButton()
    {
        if (_selectItem.item.Type == ItemType.Equipped)
        {
            _equipmentManager.EquipItem(_selectItem);
        }
    }

    public void OnDropButton()
    {
        if (_selectItem == null || _selectItem.item == null)
        {
            Debug.LogError("Попытка выбросить пустой слот!");
            return;
        }

        try
        {
            // Определяем, откуда мы удаляем предмет
            bool isFromEquipment = _equipmentManager.IsItemEquipped(_selectItem.item);

            if (isFromEquipment)
            {
                Debug.Log($"Выбрасываем экипированный предмет: {_selectItem.item.ItemName}");
                // Создаем копию предмета перед удалением
                ItemDataBase itemToDrop = _selectItem.item;

                // Удаляем из экипировки
                if (_equipmentManager.DropEquippedItem(itemToDrop))
                {
                    // Выбрасываем предмет в мир
                    ThrowItem(itemToDrop);
                    ClearSelectedItemWindow();
                }
            }
            else
            {
                Debug.Log($"Выбрасываем предмет из инвентаря: {_selectItem.item.ItemName}");
                // Создаем копию предмета перед удалением
                ItemDataBase itemToDrop = _selectItem.item;

                // Удаляем из инвентаря
                RemoveSelectedItem();

                // Выбрасываем предмет в мир
                ThrowItem(itemToDrop);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка при выкидывании предмета: {e.Message}");
        }
    }

    private void RemoveSelectedItem()
    {
        if (_selectItem == null) return;

        _selectItem.Quantity--;

        if (_selectItem.Quantity <= 0)
        {
            // Если предмет был экипирован - снимаем его
            if (_selectItem.IsEquipped)
            {
                _equipmentManager.UnequipItem(_selectItem.item);
            }

            _selectItem.item = null;
            _selectItem.Quantity = 0;
            _selectItem.IsEquipped = false;
            ClearSelectedItemWindow();
        }

        UpdateUi();
    }

    public void SelectEquipmentItem(ItemSlot equipmentSlot)
    {
        if (equipmentSlot == null || equipmentSlot.item == null)
            return;

        _selectItem = equipmentSlot;
        _selectedItemName.text = _selectItem.item.ItemName;
        _selectedItemDiscription.text = _selectItem.item.Discription;
        _selectedItemStatsName.text = string.Empty;
        _selectedItemStatsNameValue.text = string.Empty;

        // Показываем статы для экипировки
        if (_selectItem.item.EquipmentItems != null && _selectItem.item.EquipmentItems.Length > 0)
        {
            var equipData = _selectItem.item.EquipmentItems[0];
            _selectedItemStatsName.text = "Атака\nЗащита\nСкорость Атаки\n";
            _selectedItemStatsNameValue.text = $"{equipData.Attack}\n{equipData.Defense}\n{equipData.AttackSpeed}\n";
        }

        // Для экипированных предметов показываем только кнопку "Снять"
        _useButton.SetActive(false);
        _equipButton.SetActive(false);
        _unequipButton.SetActive(true);
        _dropButton.SetActive(true);
    }
    public void RemoveItem(ItemDataBase item)
    {

    }

    public bool HasItems(ItemDataBase item, int quantity)
    {
        return false;
    }

}

