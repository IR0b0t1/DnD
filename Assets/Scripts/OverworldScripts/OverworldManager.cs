using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public GameObject player;

    [Header("Background Music")]
    public AudioClip overworldBGM;
    [Range(0f, 1f)]
    public float bgmVolume = 0.5f;
    private AudioSource bgmAudioSource;

    public static OverworldManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgmAudioSource = GetComponent<AudioSource>();
            if (bgmAudioSource == null)
            {
                bgmAudioSource = gameObject.AddComponent<AudioSource>();
                bgmAudioSource.playOnAwake = false;
                bgmAudioSource.loop = true;
                Debug.LogWarning("OverworldManager: No AudioSource found for BGM, added one automatically.", this);
            }
            bgmAudioSource.volume = bgmVolume;
            bgmAudioSource.clip = overworldBGM;

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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Overworld")
        {
            FindPlayerInScene();
            ApplyPlayerPosition();
            HandleDefeatedEnemies();
            PlayOverworldBGM();
        }
        else
        {
            StopOverworldBGM();
        }
    }

    private void FindPlayerInScene()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("OverworldManager: Player GameObject with 'Player' tag not found in the scene!");
        }
    }

    private void ApplyPlayerPosition()
    {
        if (player != null)
        {
            Debug.Log("OverworldManager: Applying player position to " + GameState.playerPosition);
            player.transform.position = GameState.playerPosition;
        }
    }

    private void HandleDefeatedEnemies()
    {
        BattleTrigger[] enemies = FindObjectsByType<BattleTrigger>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            if (GameState.defeatedEnemies.Contains(enemy.enemyID))
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    public void PlayOverworldBGM()
    {
        if (bgmAudioSource != null && overworldBGM != null)
        {
            if (!bgmAudioSource.isPlaying || bgmAudioSource.clip != overworldBGM)
            {
                bgmAudioSource.clip = overworldBGM;
                bgmAudioSource.volume = bgmVolume;
                bgmAudioSource.Play();
                Debug.Log("Overworld BGM started.");
            }
        }
        else
        {
            Debug.LogWarning("OverworldManager: BGM AudioSource or Clip not assigned. Cannot play background music.", this);
        }
    }

    public void StopOverworldBGM()
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
            Debug.Log("Overworld BGM stopped.");
        }
    }
}