using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    [Header("Audio")]
    // To jest d�wi�k mojej pora�ki, kiedy widz� 20 b��d�w w konsoli po zmianie lokalizacji jednego pliku. (Tak faktycznie by�o, dlatego pliki przeciwnik�w s� w UI (NIE RUSZA� ICH BO WSZYSTKO SZLAG TRAFI!!!)
    public AudioClip defeatBGM;
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
            Debug.LogWarning("DefeatMenu: No AudioSource found, added one automatically.", this);
        }
    }

    void Start()
    {
        if (audioSource != null && defeatBGM != null)
        {
            audioSource.clip = defeatBGM;
            audioSource.volume = bgmVolume;
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