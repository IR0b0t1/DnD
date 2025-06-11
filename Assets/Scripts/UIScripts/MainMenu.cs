using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip mainMenuBGM;
    [Range(0f, 1f)]
    public float bgmVolume = 0.5f;
    private AudioSource audioSource;

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
    }

    void Start()
    {
        if (audioSource != null && mainMenuBGM != null)
        {
            audioSource.clip = mainMenuBGM;
            audioSource.volume = bgmVolume;
            audioSource.Play();
            Debug.Log("Main Menu BGM started.");
        }
        else
        {
            Debug.LogWarning("MainMenu: Main Menu BGM AudioSource or Clip not assigned. Cannot play background music.", this);
        }
    }

    public void OnPlayButton()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Main Menu BGM stopped.");
        }

        SceneManager.LoadScene("Overworld");
    }

    public void OnSettingsButton()
    {
        Debug.Log("Settings menu not implemented yet.");
    }

    public void OnQuitButton()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}