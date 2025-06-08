using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            BattleData.enemyType = other.name;
            SceneManager.LoadScene("BattleScene");
        }
    }
}
