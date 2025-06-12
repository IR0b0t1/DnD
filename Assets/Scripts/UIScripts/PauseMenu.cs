using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [Header("UI Panels")]
    public GameObject settingsPanel;

    private Button resumeButton;
    private Button settingsButton;
    private Button exitToMainMenuButton;
    private Button closeSettingsButton;

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
            return;
        }
        if (pauseMenuPanel != null) { pauseMenuPanel.SetActive(false); }
        if (settingsPanel != null) { settingsPanel.SetActive(false); }
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("PauseMenu: Awake completed. DontDestroyOnLoad set.");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("PauseMenu: OnEnable called. Subscribed to SceneLoaded event.");
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (resumeButton != null) resumeButton.onClick.RemoveAllListeners();
        if (settingsButton != null) settingsButton.onClick.RemoveAllListeners();
        if (exitToMainMenuButton != null) exitToMainMenuButton.onClick.RemoveAllListeners();
        if (closeSettingsButton != null) closeSettingsButton.onClick.RemoveAllListeners();
        Debug.Log("PauseMenu: OnDisable called. Unsubscribed from SceneLoaded and removed button listeners.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"PauseMenu: OnSceneLoaded called for scene: {scene.name}");

        if (scene.name == "Overworld")
        {
            Debug.Log("PauseMenu: Current scene is Overworld. Attempting to find and connect UI.");
            FindAndConnectAllUIElements();

            if (SettingsManager.Instance != null && settingsPanel != null)
            {
                SettingsManager.Instance.SetSettingsPanel(settingsPanel);
                Debug.Log("PauseMenu: SettingsPanel reference passed to SettingsManager.");
            }
            else if (SettingsManager.Instance == null)
            {
                Debug.LogWarning("PauseMenu: SettingsManager.Instance not found. Cannot pass SettingsPanel reference.");
            }
            else if (settingsPanel == null)
            {
                Debug.LogWarning("PauseMenu: Settings Panel is not assigned/found for PauseMenu. Cannot pass reference.");
            }

            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            Debug.Log("PauseMenu: Overworld UI initialization complete.");
        }
        else
        {
            Debug.Log($"PauseMenu: Not Overworld scene ({scene.name}). Clearing UI references.");
            pauseMenuPanel = null;
            settingsPanel = null;
            resumeButton = null;
            settingsButton = null;
            exitToMainMenuButton = null;
            closeSettingsButton = null;

            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    private void FindAndConnectAllUIElements()
    {
        GameObject pauseMenuCanvasGO = GameObject.Find("PauseMenuCanvas");
        if (pauseMenuCanvasGO == null)
        {
            Debug.LogError("PauseMenu: 'PauseMenuCanvas' GameObject NOT FOUND in the current scene ('Overworld'). All UI setup will fail!", this);
            return;
        }
        Debug.Log("PauseMenu: 'PauseMenuCanvas' found.");

        if (pauseMenuPanel == null)
        {
            Transform panelTransform = pauseMenuCanvasGO.transform.Find("PauseMenuPanel");
            if (panelTransform != null)
            {
                pauseMenuPanel = panelTransform.gameObject;
                Debug.Log("PauseMenu: 'PauseMenuPanel' found as child of Canvas.");
            }
            else
            {
                Debug.LogWarning("PauseMenu: 'PauseMenuPanel' NOT FOUND as child of 'PauseMenuCanvas'. Check name and hierarchy.", this);
            }
        }
        else
        {
            Debug.Log("PauseMenu: 'pauseMenuPanel' already assigned in Inspector. Using that reference.");
        }

        if (settingsPanel == null)
        {
            Transform settingsPanelTransform = pauseMenuCanvasGO.transform.Find("SettingsPanel");
            if (settingsPanelTransform != null)
            {
                settingsPanel = settingsPanelTransform.gameObject;
                Debug.Log("PauseMenu: 'SettingsPanel' found as child of Canvas.");
            }
            else
            {
                Debug.LogWarning("PauseMenu: 'SettingsPanel' NOT FOUND as child of 'PauseMenuCanvas'. Check name and hierarchy.", this);
            }
        }
        else
        {
            Debug.Log("PauseMenu: 'settingsPanel' already assigned in Inspector. Using that reference.");
        }

        if (pauseMenuPanel != null)
        {
            Debug.Log("PauseMenu: Attempting to connect buttons within PauseMenuPanel.");

            resumeButton = pauseMenuPanel.transform.Find("ResumeButton")?.GetComponent<Button>();
            settingsButton = pauseMenuPanel.transform.Find("SettingsButton")?.GetComponent<Button>();
            exitToMainMenuButton = pauseMenuPanel.transform.Find("ExitButton")?.GetComponent<Button>();

            AddButtonListener(resumeButton, Resume, "ResumeButton");
            AddButtonListener(settingsButton, OnSettingsButton, "SettingsButton");
            AddButtonListener(exitToMainMenuButton, ExitToMainMenu, "ExitButton");

        }
        else
        {
            Debug.LogError("PauseMenu: 'pauseMenuPanel' is NULL. Cannot connect its child buttons.", this);
        }

        if (settingsPanel != null)
        {
            Debug.Log("PauseMenu: Attempting to connect Close Settings Button within SettingsPanel.");
            closeSettingsButton = settingsPanel.transform.Find("CloseButton")?.GetComponent<Button>();
            AddButtonListener(closeSettingsButton, OnCloseSettingsButton, "CloseButton (in SettingsPanel)");
        }
        else
        {
            Debug.LogWarning("PauseMenu: 'settingsPanel' is NULL. Cannot connect Close Settings Button.", this);
        }
    }

    private void AddButtonListener(Button button, UnityEngine.Events.UnityAction action, string buttonName)
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
            Debug.Log($"PauseMenu: Listener SUCCESSFULLY added to {buttonName}.");
        }
        else
        {
            Debug.LogWarning($"PauseMenu: {buttonName} NOT FOUND or is NULL. Cannot add listener.", this);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                OnCloseSettingsButton();
            }
            else if (pauseMenuPanel != null)
            {
                if (isPaused)
                    Resume();
                else
                    Pause();
            }
            else
            {
                Debug.LogWarning("PauseMenu: Escape pressed but pauseMenuPanel reference is missing. Ensure it's assigned or found.");
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
        SettingsManager.Instance?.PlayButtonClickSound();
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
        SettingsManager.Instance?.PlayButtonClickSound();
    }

    public void OnSettingsButton()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            Debug.Log("Settings menu opened from Pause Menu.");
        }
        else
        {
            Debug.LogWarning("PauseMenu: Settings Panel is not assigned/found. Cannot open settings menu.");
        }
        SettingsManager.Instance?.PlayButtonClickSound();
    }

    public void OnCloseSettingsButton()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
            Debug.Log("Settings menu closed. Back to Pause Menu.");
        }
        else
        {
            Debug.LogWarning("PauseMenu: Pause Menu Panel is not assigned/found. Cannot return to pause menu.");
        }
        SettingsManager.Instance?.PlayButtonClickSound();
    }

    public void ExitToMainMenu()
    {
        Debug.Log("Exiting to Main Menu...");
        Time.timeScale = 1f;
        isPaused = false;
        GameState.playerPosition = new Vector3(0, 0, 0);
        SceneManager.LoadScene("MainMenu");
        SettingsManager.Instance?.PlayButtonClickSound();
    }

    public void OnSaveButton()
    {
        Debug.Log("Saving game functionality not implemented yet.");
        SettingsManager.Instance?.PlayButtonClickSound();
    }
}