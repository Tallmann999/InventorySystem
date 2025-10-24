using UnityEngine;

public class ItemObject : MonoBehaviour,IInteractable
{
    [SerializeField] private ItemDataBase item;

    public string GetInteractPrompt()
    {
        return string.Format("������� {0}", item.ItemName);
    }

    public void OnInteract()
    {
        if (item == null)
        {
            Debug.LogError("ItemDataBase �� �������� � ����������!", this);
            return;
        }

        if (Inventory.Instanse == null)
        {
            Debug.LogError("Inventory instance �� ������!");
            return;
        }
        // ������� ��������� � ���������, ����� ����������
        Inventory.Instanse.AddItem(item);
            Destroy(gameObject);        
    }
}
