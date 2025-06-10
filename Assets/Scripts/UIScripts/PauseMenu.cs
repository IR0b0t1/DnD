using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuUI;

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
            FindPauseMenuUI();
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false);
            }
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false);
            }
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    private void FindPauseMenuUI()
    {
        pauseMenuUI = GameObject.Find("PauseMenuPanel");
        if (pauseMenuUI == null)
        {
            Debug.LogWarning("PauseMenu: PauseMenuUI GameObject not found in the current scene ('Overworld'). " +
                             "Make sure it exists and is named 'PauseMenuUI'.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI != null)
            {
                if (isPaused)
                    Resume();
                else
                    Pause();
            }
            else
            {
                Debug.LogWarning("PauseMenu: Cannot toggle pause menu as PauseMenuUI reference is missing.");
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            Debug.Log("Game Resumed.");
        }
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
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