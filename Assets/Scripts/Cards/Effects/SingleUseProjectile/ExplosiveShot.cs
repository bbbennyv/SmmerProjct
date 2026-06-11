using UnityEngine;

public class ExplosiveShot : RuntimeCardEffect
{
    private float explosionRadius;

    private float explosionKnockbackMultiplier;

    private GameObject explosionEffect;

    public void Init(float radius, float kbMult, GameObject effect)
    {
        explosionRadius = radius;
        explosionKnockbackMultiplier = kbMult;
        explosionEffect = effect;
    }
    public override void OnHitProjectile(ProjectileContext p, Collider2D other)
    {
        Explode(p);

        ConsumeUse();
    }

    private void Explode(ProjectileContext p)
    {
        if (usesRemaining <= 0) return;
        Instantiate(explosionEffect, p.prefab.transform.position, Quaternion.identity);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.transform.IsChildOf(p.owner) || hit.transform == p.owner)
                continue;

            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                HealthSystem health = rb.GetComponent<HealthSystem>();

                if (health != null)
                {
                    Vector2 explosionDirection = (hit.transform.position - transform.position).normalized;
                    float explosionKnockback = p.kb * explosionKnockbackMultiplier;

                    health.TakeDamage(
                        p.dmg,
                        explosionKnockback,
                        explosionDirection,
                        rb,
                        p.chargeRat
                    );
                }
            }
        }

    }
}

