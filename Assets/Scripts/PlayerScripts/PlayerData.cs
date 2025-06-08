using UnityEngine;

public static class PlayerData
{
    public static int level = 1;
    public static int maxHealth = 100;
    public static int currentHealth = 100;

    public static int attack = 10;
    public static int strength = 10;
    public static int intelligence = 10;
    public static int agility = 10;
    public static int luck = 5;

    public static int experience = 0;
    public static int experienceToNextLevel = 100;

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

        strength += 2;
        intelligence += 2;
        agility += 1;
        luck += 1;
        attack += 2;

        maxHealth += 20;
        currentHealth = maxHealth;

        Debug.Log("Level Up! Now Level " + level);
    }
}
