using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/WeaponSpawnEffect")]
public class WeaponSpawnEffect : CardEffect
{
    [SerializeField] private WeaponData weaponData;
    public override void Execute(GameObject user)
    {
        user.GetComponent<PlayerController>()?.EquipWeapon(weaponData);
    }
}
