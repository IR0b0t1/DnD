using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip mainMenuBGM;
    public AudioMixerGroup musicMixerGroup;
    private AudioSource audioSource;

    [Header("UI Panels")]
    public GameObject settingsPanel;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            Debug.LogWarning("MainMenu: No AudioSource found, added one automatically.", this);
        }

        if (musicMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = musicMixerGroup;
        }
        else
        {
            Debug.LogWarning("MainMenu: Music Mixer Group not assigned. Volume control via mixer might not work.");
        }
    }

    void Start()
    {
        if (SettingsManager.Instance != null && settingsPanel != null)
        {
            SettingsManager.Instance.SetSettingsPanel(settingsPanel);
            Debug.Log("MainMenu: SettingsPanel reference passed to SettingsManager.");
        }
        else if (SettingsManager.Instance == null)
        {
            Debug.LogWarning("MainMenu: SettingsManager.Instance not found. Cannot pass SettingsPanel reference.");
        }
        else if (settingsPanel == null)
        {
            Debug.LogWarning("MainMenu: Settings Panel is not assigned in Inspector. Cannot pass reference.");
        }

        if (audioSource != null && mainMenuBGM != null)
        {
            audioSource.clip = mainMenuBGM;
            audioSource.Play();
            Debug.Log("Main Menu BGM started.");
        }
        else
        {
            Debug.LogWarning("MainMenu: Main Menu BGM AudioSource or Clip not assigned. Cannot play background music.", this);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void OnPlayButton()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Main Menu BGM stopped.");
        }
        PlayerData.ResetCharacter();
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ResetInventory();
        }
        else
        {
            Debug.LogWarning("InventoryManager.Instance not found. Inventory not reset.");
        }
        SceneManager.LoadScene("Overworld");
    }

    public void OnSettingsButton()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            Debug.Log("Settings menu opened.");
        }
        else
        {
            Debug.LogWarning("MainMenu: Settings Panel is not assigned. Cannot open settings menu.");
        }
    }

    public void OnCloseSettingsButton()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            Debug.Log("Settings menu closed.");
        }
        else
        {
            Debug.LogWarning("MainMenu: Settings Panel is not assigned. Cannot close settings menu.");
        }
    }

    public void OnQuitButton()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}