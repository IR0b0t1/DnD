using UnityEngine;

public class Chest : MonoBehaviour
{
    public EquipmentItem itemToGive;
    public string messageText = "You found an item!";
    private bool isPlayerNear = false;
    private bool isOpened = false;
    [SerializeField] private Sprite openSprite;
    private SpriteRenderer spriteRenderer;
    public GameObject messageBoxUI;
    public TMPro.TextMeshProUGUI messageTextUI;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isPlayerNear && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        if (isOpened) return;
        isOpened = true;

        spriteRenderer.sprite = openSprite;
        InventoryManager.Instance.AddItem(itemToGive);
        ShowMessage($"You found: {itemToGive.itemName}!");
    }

    private void ShowMessage(string text)
    {
        if (messageBoxUI != null && messageTextUI != null)
        {
            messageBoxUI.SetActive(true);
            messageTextUI.text = text;
            Invoke("HideMessage", 2f);
        }
    }

    private void HideMessage()
    {
        if (messageBoxUI != null)
            messageBoxUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = false;
    }
}
