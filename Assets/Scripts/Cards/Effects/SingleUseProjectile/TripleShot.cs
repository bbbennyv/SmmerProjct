using UnityEngine;

public class TripleShot : RuntimeCardEffect
{
    private int projectileNumber = 2;
    private float spread = 20f;
    public override void OnProjectileFired(ProjectileContext p)
    {
        for (int i = 0; i < projectileNumber; i++)
        {
            float angle = Random.Range(-spread, spread);

            Vector2 dir = Quaternion.Euler(0,0, angle) * p.direction;
            SpawnArrow(dir, p);
        }
        ConsumeUse();
    }

    private void SpawnArrow(Vector2 direction, ProjectileContext p)
    {
        GameObject projectile = Instantiate(p.prefab, transform.position, Quaternion.identity);

        projectile.GetComponent<Projectile>().InitializeProjectile(direction, p.kb, p.dmg,p.chargeRat, p.owner, p.projectileMult);
    }
}

[CreateAssetMenu(menuName = "CardEffects/Runtime/TripleShot")]
public class TripleShotEffectSO : RuntimeEffectSO
{
    public override RuntimeCardEffect CreateRuntimeEffect(GameObject user)
    {
        var effect = user.AddComponent<TripleShot>();

        return effect;
    }
}