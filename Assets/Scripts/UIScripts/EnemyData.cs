using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite enemySprite;
    public int maxHP;
    public int minAttackPower;
    public int maxAttackPower;
    public int expReward;
    public int goldReward;
    public bool canStun;
}
