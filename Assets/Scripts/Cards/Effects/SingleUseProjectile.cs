using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/SingleUseProjectile")]
public class SingleUseProjectile : CardEffect
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private RuntimeEffectSO effect;

    public override void Execute(GameObject user)
    {
        user.GetComponent<PlayerController>().EquipWeapon(weaponData);

        var runtime = effect.CreateRuntimeEffect(user);
        user.GetComponent<CardEffectHandler>().Register(runtime);
    }
}
