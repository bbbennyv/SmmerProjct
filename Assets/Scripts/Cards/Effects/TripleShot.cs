using UnityEngine;

public class TripleShot : RuntimeCardEffect
{
    private int projectileNumber = 2;
    private float spread = 20f;
    public override void OnProjectileFired(RangedWeapon rangedWeapon, Projectile projectile)
    {
        for (int i = 0; i < projectileNumber; i++)
        {
            float angle = Random.Range(-spread, spread);

            Vector2 dir = Quaternion.Euler(0,0, angle) * rangedWeapon.GetFireDirection();
            SpawnArrow(dir, rangedWeapon);
        }
        ConsumeUse();
    }

    private void SpawnArrow(Vector2 direction, RangedWeapon rangedWeapon)
    {
        GameObject projectile = Instantiate(rangedWeapon.GetProjectilePrefab(), transform.position, Quaternion.identity);

        float knockback = Mathf.Lerp(rangedWeapon.GetWeaponData().minKnockback, rangedWeapon.GetWeaponData().maxKnockback, rangedWeapon.GetChargeRatio());
        int damage = (int)Mathf.Lerp(rangedWeapon.GetWeaponData().minDamage, rangedWeapon.GetWeaponData().maxDamage, rangedWeapon.GetChargeRatio());

        projectile.GetComponent<Projectile>().InitializeProjectile(direction, knockback, damage, rangedWeapon.GetChargeRatio(), rangedWeapon.GetOwnerTransform(), rangedWeapon.GetProjectileSpeedMultiplier());
    }
}
