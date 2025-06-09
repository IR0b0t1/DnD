using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    public EquipmentItem item;
    public Image iconImage;
    public TextMeshProUGUI itemNameText;

    public void Setup(EquipmentItem newItem)
    {
        item = newItem;
        iconImage.sprite = item.icon;
        itemNameText.text = item.itemName;
    }

    public void OnClick()
    {
        InventoryManager.Instance.OnItemSelected(item);
    }
}
