using UnityEngine;

public abstract class RuntimeCardEffect : MonoBehaviour
{
    public int usesRemaining = 1;
    public virtual void OnAttack() { }
    public virtual void OnProjectileFired(RangedWeapon rangedWeapon, Projectile projectile) { }
    public virtual void OnHit() { }

    public virtual RuntimeCardEffect CreateInstance()
    {
        return Instantiate(this);
    }

    public bool ConsumeUse()
    {
        if (usesRemaining < 0)
            return false;

        usesRemaining--;

        if (usesRemaining <= 0)
        {
            Destroy(this);
            return true;
        }

        return false;
    }
}
