using UnityEngine;

public class BowWeapon : RangedWeapon
{
    [SerializeField] private float minDrawAngle = 0f;
    [SerializeField] private float maxDrawAngle = -35f;

    [SerializeField] private float rotateLerpSpeed = 12f;

    [SerializeField] private float minProjectileSpeed = 0.5f;
    [SerializeField] private float maxProjectileSpeed = 1f;

    private float baseAngle;
    private float currentAngle;


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
        if (!CanUse()) return;
        //if (chargeRatio < 0.9f) return;

        float speed = Mathf.Lerp(minProjectileSpeed, maxProjectileSpeed, chargeRatio);

        FireProjectile(speed);

        ResetCooldown();
    }

    public override bool CanCharge()
    {
        return true;
    }
}
