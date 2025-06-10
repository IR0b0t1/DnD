using UnityEngine;

[CreateAssetMenu(fileName = "ChestScriptableObject", menuName = "Chest/ChestObject")]
public class ChestScriptableObject : ScriptableObject
{
    [Header("Chest Data")]
    public EquipmentItem itemToGive;
    public string initialMessage = "You found an item!";
}
