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

        //launchAngle = Mathf.Lerp(30.0f, 5.0f, chargeRatio);
        //launchAngle = FinalAngle(launchAngle);

        projectileSpeedMult = Mathf.Lerp(minProjectileSpeed, maxProjectileSpeed, chargeRatio);
        
        base.Use();
    }

    public override Vector2 GetFireDirection()
    {
        float angle = Mathf.Lerp(30.0f, 5.0f, chargeRatio);
        angle = FinalAngle(angle);
        return Quaternion.Euler(0, 0 , angle) * hitDirection.normalized;
    }

    public override float FinalAngle(float angle)
    {
        if (hitDirection.x < 0)
        {
            angle = -angle;
        }
        return angle;
    }

    public override bool CanCharge()
    {
        return true;
    }
}
