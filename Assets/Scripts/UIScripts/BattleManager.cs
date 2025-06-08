using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image enemySpriteRenderer;
    public TextMeshProUGUI textbox;
    public Sprite frogSprite;
    public Sprite goblinSprite;

    private int playerHP = 100;
    private int enemyHP = 50;
    private bool playerTurn = true;

    void Start()
    {
        SetEnemySprite();
        StartCoroutine(ShowText("A " + BattleData.enemyType + " appears!"));
    }

    void SetEnemySprite()
    {
        if (BattleData.enemyType == "Frog")
            enemySpriteRenderer.sprite = frogSprite;
        else if (BattleData.enemyType == "Goblin")
            enemySpriteRenderer.sprite = goblinSprite;
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

    public void OnMagicButton()
    {
        if (playerTurn)
        {
            int damage = Random.Range(15, 25);
            enemyHP -= damage;
            StartCoroutine(ShowText("You cast a spell for " + damage + " damage!"));

            if (enemyHP <= 0)
                StartCoroutine(ShowText("Enemy defeated!"));
            else
                StartCoroutine(EnemyTurn());
        }
    }

    public void OnItemButton()
    {
        StartCoroutine(ShowText("You rummage through your bag... Nothing happens."));
    }

    public void OnEscapeButton()
    {
        StartCoroutine(EscapeRoutine());
    }

    IEnumerator EscapeRoutine()
    {
        yield return ShowText("You try to escape...");
        yield return new WaitForSeconds(1f);

        bool escaped = Random.value < 0.5f;
        if (escaped)
        {
            yield return ShowText("You got away!");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Overworld");
        }
        else
        {
            yield return ShowText("Escape failed!");
            StartCoroutine(EnemyTurn());
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
        yield return new WaitForSeconds(1f);
        yield return ShowText(BattleData.enemyType + " attacks!");

        int damage = Random.Range(5, 15);
        playerHP -= damage;
        yield return ShowText("You took " + damage + " damage!");

        playerTurn = true;
    }

    IEnumerator ShowText(string message)
    {
        textbox.text = message;
        yield return new WaitForSeconds(1.5f);
    }
}
