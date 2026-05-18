using UnityEngine;

public class RangedWeapon : BaseWeapon
{
    [SerializeField]
    private GameObject projectilePrefab;

    protected Transform ownerTransform;


    private void Start()
    {
        PlayerController owner = GetComponentInParent<PlayerController>();
        ownerTransform = owner.GetComponent<Transform>();
    }

    public override void Use()
    {
        if(!CanUse()) return;


        FireProjectile(1f);

        ResetCooldown();
    }


    public virtual void FireProjectile(float speedMult)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector2 direction = hitDirection;

        float knockback = Mathf.Lerp(weaponData.minKnockback, weaponData.maxKnockback, chargeRatio);
        int damage = (int)Mathf.Lerp(weaponData.minDamage, weaponData.maxDamage, chargeRatio);
        projectile.GetComponent<Projectile>().InitializeProjectile(direction, knockback, damage, chargeRatio, ownerTransform, speedMult);
    }
}
