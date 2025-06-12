using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTrigger : MonoBehaviour
{
    [Header("Ending Scene")]
    public string sceneName = "EndingScene";

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            PlayerData.ResetCharacter();
            SceneManager.LoadScene(sceneName);
        }
    }
}
