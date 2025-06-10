using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        Debug.Log("OverworldManager: Setting player position to " + GameState.playerPosition);
        player.transform.position = GameState.playerPosition;

        BattleTrigger[] enemies = FindObjectsOfType<BattleTrigger>();

        foreach (var enemy in enemies)
        {
            Debug.Log("Checking enemy: " + enemy.enemyID);
            if (GameState.defeatedEnemies.Contains(enemy.enemyID))
            {
                Debug.Log("Enemy " + enemy.enemyID + " has been defeated. Disabling GameObject.");
                enemy.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Enemy " + enemy.enemyID + " is active.");
            }
        }
    }
}