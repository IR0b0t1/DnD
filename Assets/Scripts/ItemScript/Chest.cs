using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    [Header("Chest Configuration")]
    public string chestID;
    public ChestScriptableObject chestData;

    [Header("Visuals")]
    [SerializeField] private Sprite openSprite;
    private SpriteRenderer spriteRenderer;

    [Header("Interaction")]
    public GameObject messageBoxPanel;
    public TextMeshProUGUI messageTextUI;
    private bool isPlayerNear = false;

    private bool isOpened = false;

    void OnValidate()
    {
        if (string.IsNullOrEmpty(chestID))
        {
            Debug.LogWarning("Chest ID is not set for " + gameObject.name + "! Please set a unique ID.", this);
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (messageBoxPanel != null)
        {
            messageBoxPanel.SetActive(false);
        }

        if (GameState.openedChests.Contains(chestID))
        {
            isOpened = true;
            spriteRenderer.sprite = openSprite;

            if (GetComponent<Collider2D>() != null)
            {
                GetComponent<Collider2D>().enabled = false;
            }
            Debug.Log($"Chest '{chestID}' was already opened. (Visual updated, collider disabled)");
            return;
        }

        isOpened = false;
        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = true;
        }
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

        if (InventoryManager.Instance != null && chestData != null && chestData.itemToGive != null)
        {
            InventoryManager.Instance.AddItem(chestData.itemToGive);
            ShowMessage($"You found: {chestData.itemToGive.itemName}!");
        }
        else
        {
            Debug.LogError("Chest: InventoryManager, ChestData, or itemToGive is null! Cannot add item.", this);
            ShowMessage("You found nothing...");
        }

        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        GameState.openedChests.Add(chestID);
        Debug.Log($"Chest '{chestID}' opened and recorded in GameState.");
    }

    private void ShowMessage(string text)
    {
        if (messageBoxPanel != null && messageTextUI != null)
        {
            Debug.Log("showing message: " + text);
            messageBoxPanel.SetActive(true);
            messageTextUI.text = text;
            CancelInvoke("HideMessage");
            Invoke("HideMessage", 2f);
        }
        else
        {
            Debug.LogWarning("Chest: Message UI references (MessageBoxPanel or MessageTextUI) are not set up in Inspector. Message: " + text, this);
        }
    }

    private void HideMessage()
    {
        if (messageBoxPanel != null)
            messageBoxPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (!isOpened)
            {
                Debug.Log($"Player near chest '{chestID}'. Press E to open.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (messageBoxPanel != null && messageBoxPanel.activeSelf)
            {
                CancelInvoke("HideMessage");
                HideMessage();
            }
        }
    }
}