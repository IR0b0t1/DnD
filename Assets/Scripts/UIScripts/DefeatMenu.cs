using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    [Header("Audio")]
    // To jest d�wi�k mojej pora�ki, kiedy widz� 20 b��d�w w konsoli po zmianie lokalizacji jednego pliku. (Tak faktycznie by�o, dlatego pliki przeciwnik�w s� w UI (NIE RUSZA� ICH BO WSZYSTKO SZLAG TRAFI!!!)
    public AudioClip defeatBGM;
    private AudioSource audioSource;
    public AudioMixerGroup mixerGroup;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            Debug.LogWarning("DefeatMenu: No AudioSource found, added one automatically.", this);
        }

        if (mixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = mixerGroup;
        }
        else
        {
            Debug.LogWarning("Chest: SFX Mixer Group not assigned for chest. Volume control via mixer might not work.", this);
        }
    }

    void Start()
    {
        if (audioSource != null && defeatBGM != null)
        {
            audioSource.clip = defeatBGM;
            audioSource.Play();
            Debug.Log("Defeat BGM started.");
        }
        else
        {
            Debug.LogWarning("DefeatMenu: Defeat BGM AudioSource or Clip not assigned. Cannot play background music.", this);
        }
    }

    public void OnRestartButton()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Defeat BGM stopped.");
        }

        GameState.playerPosition = new Vector3(0, 0, 0);
        SceneManager.LoadScene("Overworld");
    }

    public void OnQuitButton()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Defeat BGM stopped.");
        }

        GameState.playerPosition = new Vector3(0, 0, 0);
        SceneManager.LoadScene("MainMenu");
    }
}