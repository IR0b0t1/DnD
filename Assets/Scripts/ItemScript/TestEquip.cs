using UnityEngine;

public class TestEquip : MonoBehaviour
{
    public EquipmentItem testWeapon;
    public EquipmentItem testArmor;
    public EquipmentItem testAccessory;

    void Start()
    {
        PlayerData.weapon = testWeapon;
        PlayerData.armor = testArmor;
        PlayerData.accessory = testAccessory;
    }
}
