using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    public void OnRestartButton()
    {
        GameState.playerPosition = new Vector3(0, 0, 0);
        SceneManager.LoadScene("Overworld");
    }

    public void OnQuitButton()
    {
        GameState.playerPosition = new Vector3(0, 0, 0);
        SceneManager.LoadScene("MainMenu");
    }
}