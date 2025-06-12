using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private GameObject inventoryPanel;
    private TMP_Dropdown weaponDropdown;
    private TMP_Dropdown ArmourDropdown;
    private TMP_Dropdown accessoryDropdown;

    [Header("Inventory Items")]
    public List<EquipmentItem> allItems;

    private EquipmentItem equippedWeapon;
    private EquipmentItem equippedArmor;
    private EquipmentItem equippedAccessory;

    [Header("Default Starting Items")]
    public EquipmentItem defaultWeapon;
    public EquipmentItem defaultArmor;
    public EquipmentItem defaultAccessory;

    [Header("Player Data UI Textboxes")]
    public TextMeshProUGUI playerDataHealthText;
    public TextMeshProUGUI playerDataAttackText;
    public TextMeshProUGUI playerDataDefenseText;
    public TextMeshProUGUI playerDataStrengthText;
    public TextMeshProUGUI playerDataIntelligenceText;
    public TextMeshProUGUI playerDataAgilityText;
    public TextMeshProUGUI playerDataLuckText;
    public TextMeshProUGUI playerDataLevelText;
    public TextMeshProUGUI playerDataExpText;
    public TextMeshProUGUI playerDataNameText;
    public TextMeshProUGUI playerDataGold;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        equippedWeapon = PlayerData.weapon;
        equippedArmor = PlayerData.armor;
        equippedAccessory = PlayerData.accessory;

        PlayerData.InitializePlayerData();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Overworld")
        {
            FindUIReferencesInScene();
            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(false);
            }
            UpdatePlayerStatsUI();
        }
    }

    private void FindUIReferencesInScene()
    {
        inventoryPanel = GameObject.Find("InventoryPanel");
        if (inventoryPanel == null) Debug.LogError("InventoryManager: InventoryPanel UI not found!");

        weaponDropdown = GameObject.Find("WeaponDropdown")?.GetComponent<TMP_Dropdown>();
        if (weaponDropdown == null) Debug.LogError("InventoryManager: WeaponDropdown UI not found!");

        ArmourDropdown = GameObject.Find("ArmourDropdown")?.GetComponent<TMP_Dropdown>();
        if (ArmourDropdown == null) Debug.LogError("InventoryManager: ArmourDropdown UI not found!");

        accessoryDropdown = GameObject.Find("AccessoryDropdown")?.GetComponent<TMP_Dropdown>();
        if (accessoryDropdown == null) Debug.LogError("InventoryManager: AccessoryDropdown UI not found!");

        playerDataHealthText = GameObject.Find("PlayerDataHealth")?.GetComponent<TextMeshProUGUI>();
        if (playerDataHealthText == null) Debug.LogError("InventoryManager: PlayerDataHealth TextMeshProUGUI not found!");
        playerDataAttackText = GameObject.Find("PlayerDataAttack")?.GetComponent<TextMeshProUGUI>();
        if (playerDataAttackText == null) Debug.LogError("InventoryManager: PlayerDataAttack TextMeshProUGUI not found!");
        playerDataDefenseText = GameObject.Find("PlayerDataDefense")?.GetComponent<TextMeshProUGUI>();
        if (playerDataDefenseText == null) Debug.LogError("InventoryManager: PlayerDataDefense TextMeshProUGUI not found!");
        playerDataStrengthText = GameObject.Find("PlayerDataStrength")?.GetComponent<TextMeshProUGUI>();
        if (playerDataStrengthText == null) Debug.LogError("InventoryManager: PlayerDataStrength TextMeshProUGUI not found!");
        playerDataIntelligenceText = GameObject.Find("PlayerDataIntelligence")?.GetComponent<TextMeshProUGUI>();
        if (playerDataIntelligenceText == null) Debug.LogError("InventoryManager: PlayerDataIntelligence TextMeshProUGUI not found!");
        playerDataAgilityText = GameObject.Find("PlayerDataAgility")?.GetComponent<TextMeshProUGUI>();
        if (playerDataAgilityText == null) Debug.LogError("InventoryManager: PlayerDataAgility TextMeshProUGUI not found!");
        playerDataLuckText = GameObject.Find("PlayerDataLuck")?.GetComponent<TextMeshProUGUI>();
        if (playerDataLuckText == null) Debug.LogError("InventoryManager: PlayerDataLuck TextMeshProUGUI not found!");
        playerDataLevelText = GameObject.Find("PlayerDataLevel")?.GetComponent<TextMeshProUGUI>();
        if (playerDataLevelText == null) Debug.LogError("InventoryManager: PlayerDataLevel TextMeshProUGUI not found!");
        playerDataExpText = GameObject.Find("PlayerDataExp")?.GetComponent<TextMeshProUGUI>();
        if (playerDataExpText == null) Debug.LogError("InventoryManager: PlayerDataExp TextMeshProUGUI not found!");
        playerDataNameText = GameObject.Find("PlayerDataName")?.GetComponent<TextMeshProUGUI>();
        if (playerDataExpText == null) Debug.LogError("InventoryManager: PlayerDataName TextMeshProUGUI not found!");
        playerDataGold = GameObject.Find("PlayerDataGold")?.GetComponent<TextMeshProUGUI>();
        if (playerDataGold == null) Debug.LogError("InventoryManager: PlayerDataGold TextMeshProUGUI not found!");

        if (inventoryPanel != null && weaponDropdown != null && ArmourDropdown != null && accessoryDropdown != null)
        {
            PopulateDropdowns();
            UpdatePlayerStatsUI();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                if (inventoryPanel.activeSelf)
                {
                    PopulateDropdowns();
                    UpdatePlayerStatsUI();
                }
            }
            else
            {
                Debug.LogWarning("InventoryManager: Inventory Panel is missing, cannot toggle.");
            }
        }
    }

    void PopulateDropdowns()
    {
        if (weaponDropdown != null) PopulateDropdown(weaponDropdown, ItemType.Weapon);
        if (ArmourDropdown != null) PopulateDropdown(ArmourDropdown, ItemType.Armor);
        if (accessoryDropdown != null) PopulateDropdown(accessoryDropdown, ItemType.Accessory);
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
                PlayerData.weapon = selectedItem;
                break;
            case ItemType.Armor:
                equippedArmor = selectedItem;
                PlayerData.armor = selectedItem;
                break;
            case ItemType.Accessory:
                equippedAccessory = selectedItem;
                PlayerData.accessory = selectedItem;
                break;
        }

        PlayerData.RecalculateAndAdjustHealth();
        UpdatePlayerStatsUI();
        Debug.Log($"Equipped {type}: {(selectedItem != null ? selectedItem.itemName : "None")}");
    }

    public void AddItem(EquipmentItem item)
    {
        if (!allItems.Contains(item))
        {
            allItems.Add(item);
            Debug.Log($"Added {item.itemName} to inventory.");
            if (inventoryPanel != null && inventoryPanel.activeSelf)
            {
                PopulateDropdowns();
                UpdatePlayerStatsUI();
            }
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

    private void UpdatePlayerStatsUI()
    {
        if (playerDataHealthText != null)
        {
            playerDataHealthText.text = $"Health: {PlayerData.currentHealth}/{PlayerData.maxHealth}";
        }
        if (playerDataAttackText != null)
        {
            playerDataAttackText.text = $"Attack: {PlayerData.GetTotalAttack()}";
        }
        if (playerDataDefenseText != null)
        {
            playerDataDefenseText.text = $"Defense: {PlayerData.GetTotalDefense()}";
        }
        if (playerDataStrengthText != null)
        {
            playerDataStrengthText.text = $"Strength: {PlayerData.GetTotalStrength()}";
        }
        if (playerDataIntelligenceText != null)
        {
            playerDataIntelligenceText.text = $"Intelligence: {PlayerData.GetTotalIntelligence()}";
        }
        if (playerDataAgilityText != null)
        {
            playerDataAgilityText.text = $"Agility: {PlayerData.GetTotalAgility()}";
        }
        if (playerDataLuckText != null)
        {
            playerDataLuckText.text = $"Luck: {PlayerData.GetTotalLuck()}";
        }
        if (playerDataLevelText != null)
        {
            playerDataLevelText.text = $"LV: {PlayerData.level}";
        }
        if (playerDataExpText != null)
        {
            playerDataExpText.text = $"Exp: {PlayerData.currentExp}/{PlayerData.expToNextLevel}";
        }
        if (playerDataNameText != null)
        {
            playerDataNameText.text = PlayerData.playerName;
        }
        if (playerDataGold != null)
        {
            playerDataGold.text = $"Gold: {PlayerData.currentGold}";
        }
    }

    public void ResetInventory()
    {
        allItems.Clear();

        equippedWeapon = null;
        equippedArmor = null;
        equippedAccessory = null;

        PlayerData.weapon = null;
        PlayerData.armor = null;
        PlayerData.accessory = null;

        if (defaultWeapon != null)
        {
            AddItem(defaultWeapon);
        }
        if (defaultArmor != null)
        {
            AddItem(defaultArmor);
        }
        if (defaultAccessory != null)
        {
            AddItem(defaultAccessory);
        }

        if (inventoryPanel != null && inventoryPanel.activeSelf)
        {
            PopulateDropdowns();
            UpdatePlayerStatsUI();
        }
        Debug.Log("Inventory has been reset and default items added/equipped.");
    }
}