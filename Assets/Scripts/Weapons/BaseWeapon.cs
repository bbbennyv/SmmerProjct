using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    protected WeaponData weaponData;
    protected bool attacking;
    protected float cooldownTimer;
    public virtual void Use() { }
    
    public virtual void Initialize( WeaponData weaponData)
    {
        this.weaponData = weaponData;
    }

    public virtual void Update()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public virtual void BeginCharge()
    {

    }

    protected bool CanUse()
    {
        return cooldownTimer <= 0;
    }

    protected void ResetCooldown()
    {
        cooldownTimer = weaponData.attackCooldown;
    }
}
