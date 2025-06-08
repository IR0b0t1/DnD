using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTrigger : MonoBehaviour
{
    [Header("Enemy Settings")]
    public EnemyData enemyData;

    [Header("Battle Scene")]
    public string battleSceneName = "BattleScene";

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            BattleData.currentEnemy = enemyData;

            SceneManager.LoadScene(battleSceneName);
        }
    }
}
