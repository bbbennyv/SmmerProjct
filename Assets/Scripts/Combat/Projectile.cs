using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 15;

    private Rigidbody2D rb;
    private int damage;
    private float knockback;
    private Vector2 direction;
    private float chargeRatio;
    private Transform ownerTransform;
    private float speedMultiplier;

    [SerializeField] public LayerMask hitLayers;
    private bool hitRegistered = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    public void FixedUpdate() 
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
