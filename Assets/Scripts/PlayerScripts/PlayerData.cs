using UnityEngine;

public static class PlayerData
{
    public static string playerName = "Jonathan Does";

    public static int level = 1;
    public static int baseMaxHealth = 30;
    public static int currentHealth = 30;
    public static int maxHealth { get; private set; }

    public static int attack = 1;
    public static int defense = 0;
    public static int strength = 1;
    public static int intelligence = 1;
    public static int agility = 1;
    public static int luck = 1;

    public static int currentExp = 0;
    public static int expToNextLevel = 100;

    public static int currentGold = 100;

    public static EquipmentItem weapon;
    public static EquipmentItem armor;
    public static EquipmentItem accessory;

    public static void InitializePlayerData()
    {
        RecalculateAndAdjustHealth();
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public static int GetBonusHeal()
    {
        return accessory != null ? accessory.bonusHeal : 0;
    }

    public static int GetTotalAttack()
    {
        int totalAttack = attack;
        if (weapon != null) totalAttack += weapon.bonusAttack;
        return totalAttack;
    }

    public static int GetTotalDefense()
    {
        int totalDefense = defense;
        if (armor != null) totalDefense += armor.bonusDefense;
        return totalDefense;
    }

    public static int GetTotalStrength() => strength + (weapon?.bonusStrength ?? 0) + (armor?.bonusStrength ?? 0) + (accessory?.bonusStrength ?? 0);
    public static int GetTotalIntelligence() => intelligence + (weapon?.bonusIntelligence ?? 0) + (armor?.bonusIntelligence ?? 0) + (accessory?.bonusIntelligence ?? 0);
    public static int GetTotalAgility() => agility + (weapon?.bonusAgility ?? 0) + (armor?.bonusAgility ?? 0) + (accessory?.bonusAgility ?? 0);
    public static int GetTotalLuck() => luck + (weapon?.bonusLuck ?? 0) + (armor?.bonusLuck ?? 0) + (accessory?.bonusLuck ?? 0);

    private static int CalculateEffectiveMaxHealth()
    {
        int effectiveMax = baseMaxHealth;

        if (weapon != null) effectiveMax += weapon.bonusHealth;
        if (armor != null) effectiveMax += armor.bonusHealth;
        if (accessory != null) effectiveMax += accessory.bonusHealth;
        return effectiveMax;
    }

    public static void RecalculateAndAdjustHealth()
    {
        int oldMaxHealth = maxHealth;
        maxHealth = CalculateEffectiveMaxHealth();

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (maxHealth > oldMaxHealth)
        {
            if (oldMaxHealth > 0 && currentHealth == oldMaxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }


    public static void GainGold(int goldAmount)
    {
        currentGold += goldAmount;
        Debug.Log("Gained " + goldAmount + " gold. Total: " + currentGold);
    }

    public static void GainExperience(int exp)
    {
        currentExp += exp;
        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private static void LevelUp()
    {
        level++;
        currentExp -= expToNextLevel;
        expToNextLevel += 50;

        attack += 2;
        defense += 1;
        strength += 2;
        intelligence += 2;
        agility += 1;
        luck += 1;

        baseMaxHealth += 20;
        RecalculateAndAdjustHealth();
        currentHealth = maxHealth;
        Debug.Log("Level Up! Now Level " + level);
    }
}