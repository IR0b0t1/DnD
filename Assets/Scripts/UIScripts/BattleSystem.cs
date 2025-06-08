using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public Image enemySpriteRenderer;
    public TextMeshProUGUI textbox;
    public Sprite frogSprite;
    public Sprite goblinSprite;

    private int playerHP = 100;
    private int enemyHP = 50;
    private bool playerTurn = true;

    void Start()
    {
        if (BattleData.enemyType == "Frog")
            enemySpriteRenderer.sprite = frogSprite;
        else if (BattleData.enemyType == "Goblin")
            enemySpriteRenderer.sprite = goblinSprite;

        StartCoroutine(ShowText("A wild " + BattleData.enemyType + " appears!"));
    }

    public void OnAttackButton()
    {
        if (playerTurn)
        {
            int damage = Random.Range(10, 20);
            enemyHP -= damage;
            StartCoroutine(PlayerAttackRoutine(damage));
        }
    }

    IEnumerator PlayerAttackRoutine(int dmg)
    {
        yield return ShowText("You attack for " + dmg + " damage!");
        yield return new WaitForSeconds(1f);

        if (enemyHP <= 0)
        {
            yield return ShowText("Enemy defeated!");
        }
        else
        {
            playerTurn = false;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return ShowText(BattleData.enemyType + " attacks!");
        int damage = Random.Range(5, 15);
        playerHP -= damage;
        yield return ShowText("You took " + damage + " damage!");

        playerTurn = true;
    }

    IEnumerator ShowText(string message)
    {
        textbox.text = message;
        yield return null;
    }
}
