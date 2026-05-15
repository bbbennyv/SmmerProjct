using UnityEngine;

public class RangedWeapon : BaseWeapon
{
    [SerializeField]
    GameObject projectilePrefab;



    public override void Use()
    {
        if(!CanUse()) return;

        FireProjectile();


        ResetCooldown();
    }


    private void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector2 direction = hitDirection;

        projectile.GetComponent<Projectile>().InitializeProjectile(direction, weaponData.damage);
    }
}
