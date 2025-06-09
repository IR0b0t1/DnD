using UnityEngine;

public static class PlayerData
{
    public static int level = 1;
    public static int maxHealth = 30;
    public static int currentHealth = 30;

    public static int attack = 1;
    public static int strength = 1;
    public static int intelligence = 1;
    public static int agility = 1;
    public static int luck = 1;

    public static int experience = 0;
    public static int experienceToNextLevel = 100;

    public static EquipmentItem weapon;
    public static EquipmentItem armor;
    public static EquipmentItem accessory;

    public static int GetBonusHeal()
    {
        return accessory != null ? accessory.bonusHeal : 0;
    }

    public static int GetTotalAttack()
    {
        int baseAttack = attack;
        if (weapon != null) baseAttack += weapon.bonusAttack;
        return baseAttack;
    }

    public static int GetTotalDefense()
    {
        int baseDefense = 0;
        if (armor != null) baseDefense += armor.bonusDefense;
        return baseDefense;
    }

    public static int GetTotalStrength() => strength + (weapon?.bonusStrength ?? 0) + (armor?.bonusStrength ?? 0);
    public static int GetTotalIntelligence() => intelligence + (weapon?.bonusIntelligence ?? 0) + (armor?.bonusIntelligence ?? 0);
    public static int GetTotalAgility() => agility + (weapon?.bonusAgility ?? 0) + (armor?.bonusAgility ?? 0);
    public static int GetTotalLuck() => luck + (weapon?.bonusLuck ?? 0) + (armor?.bonusLuck ?? 0);

    public static void GainExperience(int exp)
    {
        experience += exp;
        while (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private static void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel += 50;

        attack += 2;
        strength += 2;
        intelligence += 2;
        agility += 1;
        luck += 1;

        maxHealth += 20;
        currentHealth = maxHealth;

        Debug.Log("Level Up! Now Level " + level);
    }
}
