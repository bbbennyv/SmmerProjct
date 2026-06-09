using UnityEngine;

public class FireballProjectile : Arrow
{
    [SerializeField] private float explosionRadius = 3f;

    [SerializeField] private float explosionKnockbackMultiplier = 1.5f;

    [SerializeField] private GameObject explosionEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitRegistered) return;
        if (((1 << other.gameObject.layer) & hitLayers) == 0) return;
        if (other.transform.IsChildOf(ownerTransform) || other.transform == ownerTransform) return;

        hitRegistered = true;

        Explode();
    }

    private void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, hitLayers);

        foreach (Collider2D hit in hits)
        {
            if (hit.transform.IsChildOf(ownerTransform) || hit.transform == ownerTransform)
                continue;

            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                HealthSystem health = rb.GetComponent<HealthSystem>();

                if (health != null)
                {
                    Vector2 explosionDirection = (hit.transform.position - transform.position).normalized;

                    float explosionKnockback = knockback * explosionKnockbackMultiplier;

                    health.TakeDamage(
                        damage,
                        explosionKnockback,
                        explosionDirection,
                        rb,
                        chargeRatio
                    );
                }
            }
        }

        Destroy(gameObject);
    }

}
