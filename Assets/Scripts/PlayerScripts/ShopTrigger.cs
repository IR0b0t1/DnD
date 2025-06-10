using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopTrigger : MonoBehaviour
{
    [Header("Shop Scene Settings")]
    public string shopSceneName = "ShopScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameState.playerPosition = other.transform.position;

            SceneManager.LoadScene(shopSceneName);
        }
    }
}