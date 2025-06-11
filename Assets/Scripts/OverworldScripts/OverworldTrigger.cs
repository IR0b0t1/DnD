using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldTrigger : MonoBehaviour
{
    [Header("Overworld Scene")]
    public string sceneName = "Overworld";

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            SceneManager.LoadScene(sceneName);
        }
    }
}
