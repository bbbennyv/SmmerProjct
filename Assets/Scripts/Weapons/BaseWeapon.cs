using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    protected WeaponData weaponData;
    protected bool attacking;
    protected float cooldownTimer;
    protected float chargeRatio;
    protected Vector2 hitDirection;
    protected Transform armPivot;

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

    public virtual void SetArmPivot(Transform pivot)
    {
        armPivot = pivot;
    }

    public virtual bool CanCharge() {  return false; }
    public virtual float FinalAngle(float angle)
    {
        if (hitDirection.x < 0)
        {
            angle += 180f;
            angle = -angle;
        }

        return angle;
    }

    public virtual bool isRanged() { return false; }

    protected bool CanUse()
    {
        return cooldownTimer <= 0;
    }

    protected void ResetCooldown()
    {
        cooldownTimer = weaponData.attackCooldown;
    }

    public virtual bool canStunOutOfHand() { return false; }
}
