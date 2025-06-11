using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Accessory
}

[CreateAssetMenu(menuName = "Item/EquipmentItem")]
public class EquipmentItem : ScriptableObject
{
    [Header("Item Details")]
    public string itemName;
    public ItemType itemType;
    public Sprite icon;
    public int price;
    public string description;

    [Header("Item Stats")]
    public int bonusAttack;
    public int bonusDefense;
    public int bonusStrength;
    public int bonusIntelligence;
    public int bonusAgility;
    public int bonusLuck;
    public int bonusHeal;
    public int bonusHealth;
}
