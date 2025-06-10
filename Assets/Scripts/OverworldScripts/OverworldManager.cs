using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public GameObject player;

    public static OverworldManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
}