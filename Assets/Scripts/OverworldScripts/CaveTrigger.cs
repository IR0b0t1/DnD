using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveTrigger : MonoBehaviour
{
    [Header("Cave Scene")]
    public string battleSceneName = "CaveScene";

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            GameState.playerPosition = other.transform.position;
            SceneManager.LoadScene(battleSceneName);
        }
    }
}
