using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI playerGoldText;
    public TextMeshProUGUI shopMessageText;
    public Transform itemParentContainer;
    public GameObject shopItemPrefab;

    [Header("Shop Inventory")]
    public List<EquipmentItem> shopInventory = new List<EquipmentItem>();

    void Start()
    {
        UpdatePlayerGoldUI();
        PopulateShopItems();
        ClearShopMessage();
        SetShopMessage("Welcome to my shop! What would you like to buy?");
    }

    void UpdatePlayerGoldUI()
    {
        playerGoldText.text = "Gold: " + PlayerData.currentGold;
    }

    void ClearShopMessage()
    {
        shopMessageText.text = "";
    }

    void SetShopMessage(string message)
    {
        shopMessageText.text = message;
    }

    void PopulateShopItems()
    {
        foreach (Transform child in itemParentContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (EquipmentItem item in shopInventory)
        {
            if (InventoryManager.Instance != null && InventoryManager.Instance.allItems.Contains(item))
            {
                Debug.Log($"Player already owns {item.itemName}. Not showing in shop.");
                continue;
            }

            GameObject itemEntryGO = Instantiate(shopItemPrefab, itemParentContainer);
            ShopItemEntry shopItemEntry = itemEntryGO.GetComponent<ShopItemEntry>();

            if (shopItemEntry != null)
            {
                shopItemEntry.Setup(item, this);
            }
        }
    }

    public void BuyItem(EquipmentItem itemToBuy)
    {
        if (PlayerData.currentGold >= itemToBuy.price)
        {
            PlayerData.currentGold -= itemToBuy.price;
            InventoryManager.Instance.AddItem(itemToBuy);
            UpdatePlayerGoldUI();
            SetShopMessage("Thanks for buying " + itemToBuy.itemName + "!");

            PopulateShopItems();
        }
        else
        {
            SetShopMessage("You don't have enough gold to buy " + itemToBuy.itemName + "!");
        }
    }

    public void OnExitShopButton()
    {
        StartCoroutine(ExitShopRoutine());
    }

    IEnumerator ExitShopRoutine()
    {
        SetShopMessage("See you again!");
        GameState.playerPosition = new Vector3(1, 0, 0);
        Debug.Log("ShopManager: Setting player position to " + GameState.playerPosition);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Overworld");
    }
}