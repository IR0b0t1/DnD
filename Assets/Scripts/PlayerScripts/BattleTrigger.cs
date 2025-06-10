using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTrigger : MonoBehaviour
{
    [Header("Enemy Settings")]
    public string enemyType = "";
    public string enemyID = "";
    public EnemyData enemyData;

    [Header("Battle Scene")]
    public string battleSceneName = "BattleScene";

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("BattleTrigger OnTriggerEnter2D called with " + enemyID);
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            if (GameState.defeatedEnemies.Contains(enemyID))
            {
                Debug.Log("This enemy (ID: " + enemyID + ") has already been defeated. Not triggering battle.");
                return;
            }

            hasTriggered = true;

            GameState.playerPosition = other.transform.position;

            BattleData.enemyType = enemyType;
            BattleData.currentEnemyID = enemyID;
            BattleData.currentEnemy = enemyData;

            SceneManager.LoadScene(battleSceneName);
        }
    }
}
