using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    protected WeaponData weaponData;
    protected PlayerController player;
    public virtual void Use() { }
    
    public virtual void Initialize( WeaponData weaponData, PlayerController player)
    {
        this.weaponData = weaponData;
        this.player = player;
    }
}
