using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [Header("Base Info")]
    public GameObject DropPrefab;
    public Sprite Icon;          
    //public EquipmentSlotType EquipSlotType;  
    public ItemType Type;         
    public Rarity Rarity;         
    public string Id;
    public string ItemName;        
    public string Discription;       
    public bool canStackable;        
    public int MaxStack = 1;      
    public int Price;
    public float Weight;

    [Header("Base Info")]
    public ConsumableItem[] ConsumableEffects;

    [Header("Equipment Data")]
    public EquipmentItem[] EquipmentItems;

    [Header("Stats Data")]
    public StatsEffect[] StatsEffects;

    [Header("Collectible Data")]
    public CollectibleItem[] CollectibleItems;

    [Header("Resource Data")]
    public ResourceItem[] ResourceItems; 
}
