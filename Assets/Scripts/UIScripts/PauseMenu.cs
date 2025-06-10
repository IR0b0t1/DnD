using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuPanel;

    private bool isPaused = false;

    public static PauseMenu Instance { get; private set; }

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
            FindpauseMenuPanel();
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(false);
            }
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(false);
            }
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    private void FindpauseMenuPanel()
    {
        pauseMenuPanel = GameObject.Find("PauseMenuPanel");
        if (pauseMenuPanel == null)
        {
            Debug.LogWarning("PauseMenu: pauseMenuPanel GameObject not found in the current scene ('Overworld'). " +
                             "Make sure it exists and is named 'pauseMenuPanel'.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuPanel != null)
            {
                if (isPaused)
                    Resume();
                else
                    Pause();
            }
            else
            {
                Debug.LogWarning("PauseMenu: Cannot toggle pause menu as pauseMenuPanel reference is missing.");
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            Debug.Log("Game Resumed.");
        }
    }

    public void Pause()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            Debug.Log("Game Paused.");
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    public void SaveGame()
    {
        Debug.Log("Saving game...");
    }

    public void LoadGame()
    {
        Debug.Log("Loading game...");
    }
}