using UnityEngine;

public class ShotgunWeapon : RangedWeapon
{
    [SerializeField] private float spread = 20f;
    [SerializeField] private int projectileNumber = 6;

    public override Vector2 GetFireDirection()
    {
        return hitDirection.x >= 0 ? Vector2.right : Vector2.left;
    }

    public override void Update()
    {
        base.Update();
        if (currentAmmo <= 0 && !reloading)
        {
            StartCoroutine(Reload());
        }
    }

    public override void FireProjectile()
    {
        for (int i = 0; i < projectileNumber; i++)
        {
            float angle = Random.Range(-spread, spread);

            Vector2 dir = Quaternion.Euler(0,0 ,angle) * GetFireDirection();
            SpawnPellet(dir);
        }
        currentAmmo--;
    }

    private void SpawnPellet(Vector2 dir)
    {
        GameObject projectile = Instantiate(projectilePrefab,transform.position,Quaternion.identity);

        float knockback = Mathf.Lerp(weaponData.minKnockback, weaponData.maxKnockback, chargeRatio);
        int damage = (int)Mathf.Lerp(weaponData.minDamage, weaponData.maxDamage, chargeRatio);

        projectile.GetComponent<Projectile>().InitializeProjectile(dir, knockback, damage, chargeRatio, ownerTransform, projectileSpeedMult);
    }

}
