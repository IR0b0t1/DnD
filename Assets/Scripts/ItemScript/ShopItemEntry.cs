using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemEntry : MonoBehaviour
{
    [Header("UI References")]
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public TextMeshProUGUI itemDescriptionText;
    public Button buyButton;

    private EquipmentItem currentItem;
    private ShopManager shopManager;

    public void Setup(EquipmentItem item, ShopManager manager)
    {
        currentItem = item;
        shopManager = manager;

        itemIcon.sprite = item.icon;
        itemNameText.text = item.itemName;
        itemPriceText.text = item.price + " Gold";
        itemDescriptionText.text = item.description;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    void OnBuyButtonClicked()
    {
        if (shopManager != null && currentItem != null)
        {
            shopManager.BuyItem(currentItem);
        }
    }
}