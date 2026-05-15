using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 15;

    private Rigidbody2D rb;
    private int damage;
    private Vector2 direction;

    [SerializeField] public LayerMask hitLayers;
    private bool hitRegistered = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void InitializeProjectile(Vector2 dir, int dmg)
    {
        direction = dir;
        damage = dmg;


    }

    public void Update() 
    {
        rb.linearVelocity = direction * speed * Time.fixedDeltaTime;

        if(hitRegistered)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitRegistered) return;
        if (((1 << other.gameObject.layer) & hitLayers) == 0) return;
       
        hitRegistered = true;

        //float knockback = Mathf.Lerp(weaponData.minKnockback, weaponData.maxKnockback, chargeRatio);
        //int damage = (int)Mathf.Lerp(weaponData.minDamage, weaponData.maxDamage, chargeRatio);
    }
}
