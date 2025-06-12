using UnityEngine;
using TMPro;
using UnityEngine.Audio;

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

    [Header("Audio")]
    public AudioClip chestOpenSFX;
    public AudioClip chestCloseSFX;
    private AudioSource audioSource;
    public AudioMixerGroup sfxMixerGroup;

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
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            Debug.LogWarning("Chest: No AudioSource found, added one automatically.", this);
        }

        if (sfxMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = sfxMixerGroup;
        }
        else
        {
            Debug.LogWarning("Chest: SFX Mixer Group not assigned for chest. Volume control via mixer might not work.", this);
        }
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
        else
        {
            Debug.Log($"Chest '{chestID}' is not opened yet. Ready for interaction.");
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

        if (audioSource != null && chestOpenSFX != null)
        {
            audioSource.PlayOneShot(chestOpenSFX);
        }

        if (InventoryManager.Instance != null && chestData != null && chestData.itemToGive != null)
        {
            InventoryManager.Instance.AddItem(chestData.itemToGive);
            Debug.Log($"ShowMessage now");
            ShowMessage($"You found: {chestData.itemToGive.itemName}!");
        }
        else
        {
            Debug.LogError("Chest: InventoryManager, ChestData, or itemToGive is null! Cannot add item.", this);
            Debug.Log($"ShowMessage now");
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
            Debug.Log("MessageBoxPanel: " + messageBoxPanel + ", MessageTextUI: " + messageTextUI);
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
        {
            messageBoxPanel.SetActive(false);
            if (audioSource != null && chestCloseSFX != null)
            {
                audioSource.PlayOneShot(chestCloseSFX);
            }
        }
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

    // Ta pozornie prosta i sensowna metoda doprowadzi³a autora do za³amania nerwowego, poniewa¿ uwali³a ca³y system wiadomoœci. Shame on you, Unity and C#!!!!!!
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isPlayerNear = false;
    //        if (messageBoxPanel != null && messageBoxPanel.activeSelf)
    //        {
    //            CancelInvoke("HideMessage");
    //            HideMessage();
    //        }
    //    }
    //}
}