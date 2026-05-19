using Unity.VisualScripting;
using UnityEngine;

public class RangedWeapon : BaseWeapon
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float recoil = 5f;

    [SerializeField]
    protected float projectileSpeedMult = 1f;

    protected Transform ownerTransform;


    private void Start()
    {
        PlayerController owner = GetComponentInParent<PlayerController>();
        ownerTransform = owner.GetComponent<Transform>();
    }

    public override void Use()
    {
        if(!CanUse()) return;

        FireProjectile();
        ApplyRecoil();

        ResetCooldown();
    }


    public virtual void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector2 direction = GetFireDirection();

        float knockback = Mathf.Lerp(weaponData.minKnockback, weaponData.maxKnockback, chargeRatio);
        int damage = (int)Mathf.Lerp(weaponData.minDamage, weaponData.maxDamage, chargeRatio);
        projectile.GetComponent<Projectile>().InitializeProjectile(direction, knockback, damage, chargeRatio, ownerTransform, projectileSpeedMult);
    }

    protected virtual Vector2 GetFireDirection()
    {
        return hitDirection.normalized;
    }

    protected virtual void ApplyRecoil()
    {
        Rigidbody2D rb = ownerTransform.gameObject.GetComponent<Rigidbody2D>();

        if(rb != null)
        {
            Vector2 recoilDirection = -hitDirection.normalized;

            rb.AddForce(recoilDirection * recoil, ForceMode2D.Impulse);
        }
    }
}
