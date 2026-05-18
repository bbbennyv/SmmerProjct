using UnityEngine;

public class RangedWeapon : BaseWeapon
{
    [SerializeField]
    private GameObject projectilePrefab;

    protected Transform ownerTransform;

    [SerializeField] private float minDrawAngle = 0f;
    [SerializeField] private float maxDrawAngle = -35f;

    [SerializeField] private float rotateLerpSpeed = 12f;

    private float baseAngle;
    private float currentAngle;

    private void Start()
    {
        PlayerController owner = GetComponentInParent<PlayerController>();
        ownerTransform = owner.GetComponent<Transform>();
    }

    public override void Update()
    {
        base.Update();
        TickChargePose();
    }

    void TickChargePose()
    {
        float eased = Mathf.SmoothStep(0, 1, chargeRatio);

        float target =
            baseAngle +
            Mathf.Lerp(minDrawAngle, maxDrawAngle, eased);

        currentAngle = Mathf.LerpAngle(
            currentAngle,
            target,
            Time.deltaTime * rotateLerpSpeed
        );

        armPivot.localRotation =
            Quaternion.Euler(0, 0, FinalAngle(currentAngle));
    }

    public override void Use()
    {
        if(!CanUse()) return;
        if (chargeRatio < 0.9f) return;

        FireProjectile();

        ResetCooldown();
    }


    private void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector2 direction = hitDirection;

        float knockback = Mathf.Lerp(weaponData.minKnockback, weaponData.maxKnockback, chargeRatio);
        int damage = (int)Mathf.Lerp(weaponData.minDamage, weaponData.maxDamage, chargeRatio);
        projectile.GetComponent<Projectile>().InitializeProjectile(direction, knockback, damage, chargeRatio, ownerTransform);
    }
}
