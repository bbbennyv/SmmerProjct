using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    protected WeaponData weaponData;
    protected bool attacking;
    protected float cooldownTimer;
    protected float chargeRatio;
    protected Vector2 hitDirection;

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

    public virtual void SetBaseAngle(Vector2 forward)
    {
        hitDirection = forward;
    }

    public virtual void SetChargeRatio(float ratio)
    {
        chargeRatio = Mathf.Clamp01(ratio);
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
