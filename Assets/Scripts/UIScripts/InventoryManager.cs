using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public GameObject inventoryPanel;
    public TMP_Dropdown weaponDropdown;
    public TMP_Dropdown armorDropdown;
    public TMP_Dropdown accessoryDropdown;

    public List<EquipmentItem> allItems;

    private EquipmentItem equippedWeapon;
    private EquipmentItem equippedArmor;
    private EquipmentItem equippedAccessory;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            if (inventoryPanel.activeSelf)
                PopulateDropdowns();
        }
    }

    void PopulateDropdowns()
    {
        PopulateDropdown(weaponDropdown, ItemType.Weapon);
        PopulateDropdown(armorDropdown, ItemType.Armor);
        PopulateDropdown(accessoryDropdown, ItemType.Accessory);
    }

    void PopulateDropdown(TMP_Dropdown dropdown, ItemType type)
    {
        dropdown.ClearOptions();

        List<string> options = new List<string> { "None" };
        List<EquipmentItem> filteredItems = allItems.FindAll(item => item.itemType == type);

        foreach (var item in filteredItems)
        {
            options.Add(item.itemName);
        }

        dropdown.AddOptions(options);
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener((index) => OnDropdownChanged(type, index, filteredItems));
        dropdown.value = GetEquippedIndex(type, filteredItems);
    }

    void OnDropdownChanged(ItemType type, int index, List<EquipmentItem> itemList)
    {
        EquipmentItem selectedItem = (index == 0) ? null : itemList[index - 1];

        switch (type)
        {
            case ItemType.Weapon:
                equippedWeapon = selectedItem;
                break;
            case ItemType.Armor:
                equippedArmor = selectedItem;
                break;
            case ItemType.Accessory:
                equippedAccessory = selectedItem;
                break;
        }

        Debug.Log($"Equipped {type}: {(selectedItem != null ? selectedItem.itemName : "None")}");
    }

    public void AddItem(EquipmentItem item)
    {
        if (!allItems.Contains(item))
        {
            allItems.Add(item);
            Debug.Log($"Added {item.itemName} to inventory.");
        }
    }

    int GetEquippedIndex(ItemType type, List<EquipmentItem> items)
    {
        EquipmentItem equipped = type switch
        {
            ItemType.Weapon => equippedWeapon,
            ItemType.Armor => equippedArmor,
            ItemType.Accessory => equippedAccessory,
            _ => null
        };

        if (equipped == null) return 0;

        int index = items.IndexOf(equipped);
        return index >= 0 ? index + 1 : 0;
    }

    public EquipmentItem GetEquippedWeapon() => equippedWeapon;
    public EquipmentItem GetEquippedArmor() => equippedArmor;
    public EquipmentItem GetEquippedAccessory() => equippedAccessory;
}
