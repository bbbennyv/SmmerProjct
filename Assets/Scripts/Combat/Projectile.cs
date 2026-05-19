using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected float speed = 15;

    protected Rigidbody2D rb;
    protected int damage;
    protected float knockback;
    protected Vector2 direction;
    protected float chargeRatio;
    protected Transform ownerTransform;
    protected float speedMultiplier;

    [SerializeField] protected LayerMask hitLayers;

    protected bool hitRegistered = false;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f);
    }

    public void InitializeProjectile(Vector2 dir, float kb, int dmg, float charge, Transform owner, float speedMult)
    {
        direction = dir.normalized;
        knockback = kb;
        damage = dmg;
        chargeRatio = charge;
        ownerTransform = owner;
        speedMultiplier = speedMult;
    }

    protected virtual void FixedUpdate() 
    {
        rb.linearVelocity = direction * speed * speedMultiplier;

        if(hitRegistered)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitRegistered) return;
        if (((1 << other.gameObject.layer) & hitLayers) == 0) return;
        if (other.transform.IsChildOf(ownerTransform) || other.transform == ownerTransform) return;


        hitRegistered = true;


        Rigidbody2D targetRb = other.GetComponent<Rigidbody2D>();
        if (targetRb != null) 
        {
            HealthSystem targetHealth = targetRb.GetComponent<HealthSystem>();
            targetHealth.TakeDamage(damage, knockback, direction, targetRb, chargeRatio);

        }
    }
}
