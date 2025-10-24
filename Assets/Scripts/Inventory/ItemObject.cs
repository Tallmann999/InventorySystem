using UnityEngine;

public class ItemObject : MonoBehaviour,IInteractable
{
    [SerializeField] private ItemDataBase item;

    public string GetInteractPrompt()
    {
        return string.Format("Поднять {0}", item.ItemName);
    }

    public void OnInteract()
    {
        if (item == null)
        {
            Debug.LogError("ItemDataBase не назначен в инспекторе!", this);
            return;
        }

        if (Inventory.Instanse == null)
        {
            Debug.LogError("Inventory instance не найден!");
            return;
        }
        // Сначала добавляем в инвентарь, потом уничтожаем
        Inventory.Instanse.AddItem(item);
            Destroy(gameObject);        
    }
}
