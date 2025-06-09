using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Image weaponSlotImage;
    public Image armorSlotImage;
    public Image accessorySlotImage;

    private EquipmentItem equippedWeapon;
    private EquipmentItem equippedArmor;
    private EquipmentItem equippedAccessory;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnItemSelected(EquipmentItem item)
    {
        switch (item.itemType)
        {
            case ItemType.Weapon:
                EquipItem(ref equippedWeapon, weaponSlotImage, item);
                break;
            case ItemType.Armor:
                EquipItem(ref equippedArmor, armorSlotImage, item);
                break;
            case ItemType.Accessory:
                EquipItem(ref equippedAccessory, accessorySlotImage, item);
                break;
        }
    }

    private void EquipItem(ref EquipmentItem slot, Image slotImage, EquipmentItem newItem)
    {
        slot = newItem;
        slotImage.sprite = newItem.icon;
        slotImage.enabled = true;
    }

    public void OnSlotClicked(string slotType)
    {
        switch (slotType)
        {
            case "Weapon":
                equippedWeapon = null;
                weaponSlotImage.enabled = false;
                break;
            case "Armor":
                equippedArmor = null;
                armorSlotImage.enabled = false;
                break;
            case "Accessory":
                equippedAccessory = null;
                accessorySlotImage.enabled = false;
                break;
        }
    }

    public EquipmentItem GetEquippedWeapon() => equippedWeapon;
    public EquipmentItem GetEquippedArmor() => equippedArmor;
    public EquipmentItem GetEquippedAccessory() => equippedAccessory;
}
