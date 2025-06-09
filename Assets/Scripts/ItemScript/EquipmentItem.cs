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
    public string itemName;
    public ItemType itemType;
    public Sprite icon;

    public int bonusAttack;
    public int bonusDefense;
    public int bonusStrength;
    public int bonusIntelligence;
    public int bonusAgility;
    public int bonusLuck;
    public int bonusHeal;
}
