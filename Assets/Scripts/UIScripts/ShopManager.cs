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

    [Header("Audio")]
    public AudioClip shopBGM;
    [Range(0f, 1f)]
    public float bgmVolume = 0.5f;
    public AudioClip shopExitDoorSFX;
    public AudioClip buySuccessSFX;
    public AudioClip buyDeclineSFX;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            Debug.LogWarning("ShopManager: No AudioSource found, added one automatically.", this);
        }
    }

    void Start()
    {
        UpdatePlayerGoldUI();
        PopulateShopItems();
        ClearShopMessage();
        SetShopMessage("Welcome to my shop! What would you like to buy?");

        if (audioSource != null && shopBGM != null)
        {
            audioSource.clip = shopBGM;
            audioSource.loop = true;
            audioSource.volume = bgmVolume;
            audioSource.Play();
            Debug.Log("Shop BGM started.");
        }
        else
        {
            Debug.LogWarning("ShopManager: Shop BGM AudioSource or Clip not assigned. Cannot play background music.", this);
        }
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

            if (audioSource != null && buySuccessSFX != null)
            {
                audioSource.PlayOneShot(buySuccessSFX);
            }
        }
        else
        {
            SetShopMessage("You don't have enough gold to buy " + itemToBuy.itemName + "!");

            if (audioSource != null && buyDeclineSFX != null)
            {
                audioSource.PlayOneShot(buyDeclineSFX);
            }
        }
    }

    public void OnExitShopButton()
    {
        StartCoroutine(ExitShopRoutine());
    }

    IEnumerator ExitShopRoutine()
    {
        SetShopMessage("See you again!");

        if (audioSource != null && shopExitDoorSFX != null)
        {
            audioSource.PlayOneShot(shopExitDoorSFX);
            yield return new WaitForSeconds(2f);
        }
        else
        {
            Debug.LogWarning("ShopManager: AudioSource or Shop Exit Door SFX not assigned. Still exiting.", this);
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        GameState.playerPosition = new Vector3(1, 0, 0);
        Debug.Log("ShopManager: Setting player position to " + GameState.playerPosition);
        SceneManager.LoadScene("Overworld");
    }
}