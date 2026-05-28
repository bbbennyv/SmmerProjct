using UnityEngine;

public abstract class RuntimeCardEffect : MonoBehaviour
{
    public int usesRemaining = 1;
    public virtual void OnAttack() { }
    public virtual void OnProjectileFired(ProjectileContext projCont) { }
    public virtual void OnHitProjectile(ProjectileContext projCont, Collider2D other) { }

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
