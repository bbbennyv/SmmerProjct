using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

[CreateAssetMenu(menuName = "CardEffects/SingleUseProjectile/TripleShot")]
public class SingleUseProjectile : CardEffect
{
    [SerializeField] private WeaponData singleUseWeapon;
    public override void Execute(GameObject user)
    {
        user.GetComponent<PlayerController>()?.EquipWeapon(singleUseWeapon);
        RuntimeCardEffect effect = user.AddComponent<TripleShot>();

    }
}
