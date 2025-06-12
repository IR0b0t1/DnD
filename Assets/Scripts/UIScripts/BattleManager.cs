using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;

public class BattleManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image enemySpriteRenderer;
    public TextMeshProUGUI textbox;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI enemyHealthText;

    [Header("Enemy Data")]
    private int enemyHP = 0;
    private int enemyMaxHP = 100;
    private int enemyMinAttackPower = 5;
    private int enemyMaxAttackPower = 15;

    [Header("Player Data")]
    private int playerHP = 0;
    private int playerMaxHP = 100;
    private int playerAttackPower = 10;
    private int playerStrenght = 10;
    private int playerIntelligence = 10;
    private int playerAgility = 10;
    private int playerLuck = 10;
    private bool playerTurn = true;
    private bool isBusy = false;

    [Header("Audio")]
    public AudioClip battleBGM;
    public AudioMixerGroup musicMixerGroup;
    public AudioClip playerAttackSFX;
    public AudioClip playerMagicSFX;
    public AudioClip enemyAttackSFX;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            Debug.LogWarning("BattleManager: No AudioSource found, added one automatically.", this);
        }

        if (musicMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = musicMixerGroup;
        }
        else
        {
            Debug.LogWarning("MainMenu: Music Mixer Group not assigned. Volume control via mixer might not work.");
        }
    }

    void Start()
    {
        enemyHP = BattleData.currentEnemy.maxHP;
        enemyMaxHP = BattleData.currentEnemy.maxHP;
        playerHP = PlayerData.currentHealth;
        playerMaxHP = PlayerData.maxHealth;
        playerAttackPower = PlayerData.GetTotalAttack();
        playerStrenght = PlayerData.GetTotalStrength();
        playerIntelligence = PlayerData.GetTotalIntelligence();
        playerAgility = PlayerData.GetTotalAgility();
        playerLuck = PlayerData.GetTotalLuck();
        enemyMinAttackPower = BattleData.currentEnemy.minAttackPower;
        enemyMaxAttackPower = BattleData.currentEnemy.maxAttackPower;

        enemySpriteRenderer.sprite = BattleData.currentEnemy.enemySprite;
        UpdateHealthUI();
        StartCoroutine(ShowText(BattleData.currentEnemy.enemyName + " appears!"));

        if (audioSource != null && battleBGM != null)
        {
            audioSource.clip = battleBGM;
            audioSource.loop = true;
            audioSource.Play();
            Debug.Log("Battle BGM started.");
        }
        else
        {
            Debug.LogWarning("BattleManager: Battle BGM AudioSource or Clip not assigned. Cannot play background music.", this);
        }
    }

    public void OnAttackButton()
    {
        if (!playerTurn || isBusy) return;
        isBusy = true;

        int damage = Random.Range(playerAttackPower, playerAttackPower + playerStrenght);
        damage += PlayerData.GetTotalAttack();
        int totalStrength = playerStrenght;
        var weapon = InventoryManager.Instance.GetEquippedWeapon();
        if (weapon != null)
            totalStrength += weapon.bonusStrength;
        damage += totalStrength * 2;
        if (playerLuck > Random.Range(0, 100))
        {
            damage *= 2;
            StartCoroutine(ShowText("Critical hit!"));
        }
        enemyHP -= damage;
        if (enemyHP < 0) enemyHP = 0;
        UpdateHealthUI();
        StartCoroutine(PlayerAttackRoutine(damage));
    }

    IEnumerator PlayerAttackRoutine(int dmg)
    {
        if (audioSource != null && playerAttackSFX != null)
        {
            audioSource.PlayOneShot(playerAttackSFX);
        }

        yield return ShowText("You attack for " + dmg + " damage!");
        yield return new WaitForSeconds(1f);

        if (enemyHP <= 0)
        {
            yield return ShowText(BattleData.currentEnemy.enemyName + " has been defeated!");
            int expGained = BattleData.currentEnemy.expReward;
            int goldGained = BattleData.currentEnemy.goldReward;
            PlayerData.GainExperience(expGained);
            PlayerData.GainGold(goldGained);
            yield return ShowText("You gained " + expGained + " EXP and " + goldGained + " gold!");
            Debug.Log("Enemy defeated: " + BattleData.currentEnemyID);
            GameState.defeatedEnemies.Add(BattleData.currentEnemyID);
            playerHP += 20;
            if (playerHP > playerMaxHP) playerHP = playerMaxHP;
            PlayerData.currentHealth = playerHP;
            yield return new WaitForSeconds(1.5f);
            EndBattle(true, false);
        }
        else
        {
            playerTurn = false;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnMagicButton()
    {
        if (!playerTurn || isBusy) return;
        isBusy = true;

        int damage = Random.Range(playerAttackPower, playerAttackPower + playerIntelligence);
        damage += playerIntelligence * 2;
        if (playerLuck > Random.Range(0, 100))
        {
            damage *= 2;
            StartCoroutine(ShowText("Critical magic hit!"));
        }
        enemyHP -= damage;
        if (enemyHP < 0) enemyHP = 0;
        UpdateHealthUI();
        StartCoroutine(MagicAttackRoutine(damage));
    }

    IEnumerator MagicAttackRoutine(int damage)
    {
        if (audioSource != null && playerMagicSFX != null)
        {
            audioSource.PlayOneShot(playerMagicSFX);
        }
        else if (audioSource != null && playerAttackSFX != null)
        {
            audioSource.PlayOneShot(playerAttackSFX);
        }

        yield return ShowText("You cast a spell for " + damage + " damage!");
        yield return new WaitForSeconds(1f);

        if (enemyHP <= 0)
        {
            yield return ShowText(BattleData.currentEnemy.enemyName + " has been defeated!");
            int expGained = BattleData.currentEnemy.expReward;
            int goldGained = BattleData.currentEnemy.goldReward;
            PlayerData.GainExperience(expGained);
            PlayerData.GainGold(goldGained);
            yield return ShowText("You gained " + expGained + " EXP and " + goldGained + " gold!");
            Debug.Log("Enemy defeated: " + BattleData.currentEnemyID);
            GameState.defeatedEnemies.Add(BattleData.currentEnemyID);
            playerHP += 20;
            if (playerHP > playerMaxHP) playerHP = playerMaxHP;
            PlayerData.currentHealth = playerHP;
            yield return new WaitForSeconds(1.5f);
            EndBattle(true, false);
        }
        else
        {
            playerTurn = false;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnItemButton()
    {
        if (!playerTurn || isBusy) return;
        isBusy = true;

        int baseHeal = Random.Range(1, 5);
        int bonusHeal = 0;
        var accessory = InventoryManager.Instance.GetEquippedAccessory();
        if (accessory != null)
            bonusHeal += accessory.bonusHeal;
        int totalHeal = baseHeal + bonusHeal;

        playerHP += totalHeal;
        if (playerHP > playerMaxHP) playerHP = playerMaxHP;

        UpdateHealthUI();
        StartCoroutine(ItemUseRoutine(totalHeal));
    }

    IEnumerator ItemUseRoutine(int healAmount)
    {
        yield return ShowText("You used a healing ring and healed " + healAmount + " HP!");
        yield return new WaitForSeconds(1f);

        playerTurn = false;
        StartCoroutine(EnemyTurn());
    }


    public void OnEscapeButton()
    {
        if (isBusy) return;
        isBusy = true;

        StartCoroutine(EscapeRoutine());
    }

    IEnumerator EscapeRoutine()
    {
        yield return ShowText("You try to escape...");
        yield return new WaitForSeconds(1f);

        bool escaped = Random.value < (playerAgility / 100f);

        if (escaped)
        {
            yield return ShowText("You got away!");
            Debug.Log("Enemy defeated: " + BattleData.currentEnemyID);
            GameState.defeatedEnemies.Add(BattleData.currentEnemyID);
            PlayerData.currentHealth = playerHP;
            yield return new WaitForSeconds(1.5f);
            EndBattle(true, true);
        }
        else
        {
            yield return ShowText("Escape failed!");
            StartCoroutine(EnemyTurn());
        }
    }


    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        if (audioSource != null && enemyAttackSFX != null)
        {
            audioSource.PlayOneShot(enemyAttackSFX);
        }

        int damage = Random.Range(enemyMinAttackPower, enemyMaxAttackPower);
        damage = Mathf.Max(0, damage - PlayerData.GetTotalDefense());

        playerHP -= damage;
        if (playerHP < 0) playerHP = 0;
        UpdateHealthUI();

        if (playerHP <= 0)
        {
            yield return ShowText(BattleData.currentEnemy.enemyName + " attacks for " + damage + " damage!");
            yield return new WaitForSeconds(1f);
            yield return ShowText("You've been defeated!");
            PlayerData.currentHealth = playerHP;
            yield return new WaitForSeconds(1f);
            EndBattle(false, false);
        }
        else
        {
            yield return ShowText(BattleData.currentEnemy.enemyName + " attacks for " + damage + " damage!");
            PlayerData.currentHealth = playerHP;
            yield return new WaitForSeconds(1f);
            yield return ShowText("Your turn");
            playerTurn = true;
            isBusy = false;
        }
    }

    void UpdateHealthUI()
    {
        playerHealthText.text = "HP: " + playerHP + " / " + playerMaxHP;
        enemyHealthText.text = "HP: " + enemyHP + " / " + enemyMaxHP;
    }

    IEnumerator ShowText(string message, System.Action onComplete = null)
    {
        textbox.text = message;
        yield return new WaitForSeconds(1.5f);
        onComplete?.Invoke();
    }

    void EndBattle(bool playerWon, bool escaped)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Battle BGM stopped.");
        }

        if (playerWon)
        {
            if(escaped)
                if(GameState.playerPosition.x < 0)
                    GameState.playerPosition += new Vector3((float)1.5, 0, 0);
                else
                    GameState.playerPosition += new Vector3((float)-1.5, 0, 0);
            SceneManager.LoadScene("Overworld");
        }
        else
        {
            GameState.playerPosition = new Vector3(0, 0, 0);
            PlayerData.currentHealth = PlayerData.maxHealth;
            SceneManager.LoadScene("DefeatScene");
        }
    }
}