using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image enemySpriteRenderer;
    public TextMeshProUGUI textbox;

    [Header("Enemy Sprites")]
    public Sprite frogSprite;
    public Sprite goblinSprite;

    [Header("Stats")]
    private int playerHP = 100;
    private int enemyHP = 50;
    private bool playerTurn = true;
    private bool isActionInProgress = false;

    void Start()
    {
        SetEnemySprite();
        StartCoroutine(ShowText("A wild " + BattleData.enemyType + " appears!"));
    }

    void SetEnemySprite()
    {
        switch (BattleData.enemyType)
        {
            case "Frog":
                enemySpriteRenderer.sprite = frogSprite;
                break;
            case "Goblin":
                enemySpriteRenderer.sprite = goblinSprite;
                break;
            default:
                Debug.LogWarning("Unknown enemy type!");
                break;
        }
    }

    public void OnAttackButton()
    {
        if (playerTurn && !isActionInProgress)
        {
            int damage = Random.Range(10, 20);
            enemyHP -= damage;
            StartCoroutine(PlayerAttackRoutine(damage));
        }
    }

    IEnumerator PlayerAttackRoutine(int damage)
    {
        isActionInProgress = true;

        yield return ShowText($"You attack for {damage} damage!");
        yield return new WaitForSeconds(1f);

        if (enemyHP <= 0)
        {
            yield return ShowText("Enemy defeated!");
            yield return new WaitForSeconds(1f);
            // TODO: Load victory screen or return to map
        }
        else
        {
            playerTurn = false;
            yield return EnemyTurn();
        }

        isActionInProgress = false;
    }

    IEnumerator EnemyTurn()
    {
        int damage = Random.Range(5, 15);
        yield return ShowText($"{BattleData.enemyType} attacks!");
        yield return new WaitForSeconds(1f);

        playerHP -= damage;
        yield return ShowText($"You took {damage} damage!");
        yield return new WaitForSeconds(1f);

        if (playerHP <= 0)
        {
            yield return ShowText("You were defeated...");
            // TODO: Handle game over
        }

        playerTurn = true;
    }

    IEnumerator ShowText(string message)
    {
        textbox.text = message;
        yield return null;
    }
}
