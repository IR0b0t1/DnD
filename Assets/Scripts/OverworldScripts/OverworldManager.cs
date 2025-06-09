using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player.transform.position = GameState.playerPosition;

        BattleTrigger[] enemies = FindObjectsOfType<BattleTrigger>();
        foreach (var enemy in enemies)
        {
            if (GameState.defeatedEnemies.Contains(enemy.enemyID))
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }
}
