using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Audio Settings")]
    public AudioMixer masterMixer;
    public string masterVolumeParam = "MasterVolume";

    [Header("UI Sound Settings")]
    public AudioClip buttonClickSFX;
    private AudioSource uiAudioSource;
    private Slider _volumeSlider;
    private TMP_Dropdown _resolutionDropdown;

    private Resolution[] resolutions;

    private const string VOLUME_PREF_KEY = "MasterVolume";
    private const string RESOLUTION_INDEX_PREF_KEY = "ResolutionIndex";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            uiAudioSource = GetComponent<AudioSource>();
            if (uiAudioSource == null)
            {
                uiAudioSource = gameObject.AddComponent<AudioSource>();
                uiAudioSource.playOnAwake = false;
                uiAudioSource.loop = false; 
                Debug.LogWarning("SettingsManager: No AudioSource found for UI sounds, added one automatically.", this);
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (_volumeSlider != null)
        {
            _volumeSlider.onValueChanged.RemoveAllListeners();
        }
        if (_resolutionDropdown != null)
        {
            _resolutionDropdown.onValueChanged.RemoveAllListeners();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _volumeSlider = null;
        _resolutionDropdown = null;

        LoadSettings();
    }

    public void SetSettingsPanel(GameObject settingsPanelGO)
    {
        if (settingsPanelGO == null)
        {
            Debug.LogWarning("SettingsManager: Received a null settingsPanelGO. Cannot setup UI elements.", this);
            return;
        }

        _volumeSlider = settingsPanelGO.GetComponentInChildren<Slider>(true);
        _resolutionDropdown = settingsPanelGO.GetComponentInChildren<TMP_Dropdown>(true);

        if (_volumeSlider != null)
        {
            _volumeSlider.onValueChanged.RemoveAllListeners();
            _volumeSlider.onValueChanged.AddListener(SetVolume);
            _volumeSlider.value = PlayerPrefs.GetFloat(VOLUME_PREF_KEY, 1f);
            Debug.Log("SettingsManager: Volume Slider found and listener added.");
        }
        else
        {
            Debug.LogWarning("SettingsManager: Volume Slider GameObject not found under the provided SettingsPanel. Make sure it exists and is a child.", this);
        }

        if (_resolutionDropdown != null)
        {
            PopulateResolutionDropdown();
            Debug.Log("SettingsManager: Resolution Dropdown found and populated.");
        }
        else
        {
            Debug.LogWarning("SettingsManager: Resolution Dropdown GameObject not found under the provided SettingsPanel. Make sure it exists and is a child.", this);
        }
    }

    public void SetVolume(float volume)
    {
        float dB;
        if (volume <= 0.0001f)
        {
            dB = -80f;
        }
        else
        {
            dB = 20f * Mathf.Log10(volume);
            dB = Mathf.Clamp(dB, -80f, 0f);
        }

        masterMixer.SetFloat(masterVolumeParam, dB);

        PlayerPrefs.SetFloat(VOLUME_PREF_KEY, volume);

        Debug.Log($"Overall Volume set to: {volume} (dB: {dB})");
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutions == null || resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
        {
            Debug.LogError("SettingsManager: Invalid resolution index or resolutions array is null.");
            return;
        }

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(RESOLUTION_INDEX_PREF_KEY, resolutionIndex);
        Debug.Log($"Resolution set to: {resolution.width}x{resolution.height}");
    }

    void PopulateResolutionDropdown()
    {
        if (_resolutionDropdown == null)
        {
            Debug.LogWarning("SettingsManager: Resolution Dropdown is not assigned/found yet. Cannot populate resolutions.", this);
            return;
        }

        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution res = resolutions[i];
            string option = res.width + "x" + res.height + " @ " + res.refreshRateRatio.value + "Hz";
            options.Add(option);

            if (res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height &&
                res.refreshRateRatio.value == Screen.currentResolution.refreshRateRatio.value)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.AddOptions(options);

        int savedResolutionIndex = PlayerPrefs.GetInt(RESOLUTION_INDEX_PREF_KEY, currentResolutionIndex);
        _resolutionDropdown.value = savedResolutionIndex;

        SetResolution(_resolutionDropdown.value);

        _resolutionDropdown.onValueChanged.RemoveAllListeners();
        _resolutionDropdown.onValueChanged.AddListener(SetResolution);

        Debug.Log("Resolution dropdown populated and listener added.");
    }

    void LoadSettings()
    {
        float savedOverallVolume = PlayerPrefs.GetFloat(VOLUME_PREF_KEY, 1f);
        SetVolume(savedOverallVolume);
    }

    public void PlayButtonClickSound()
    {
        if (uiAudioSource != null && buttonClickSFX != null)
        {
            uiAudioSource.PlayOneShot(buttonClickSFX);
            Debug.Log("Button click sound played.");
        }
        else
        {
            Debug.LogWarning("SettingsManager: UI AudioSource or Button Click SFX is not assigned. Cannot play button sound.", this);
        }
    }
}