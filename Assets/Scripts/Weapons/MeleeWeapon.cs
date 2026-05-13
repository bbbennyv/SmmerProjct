using NUnit.Framework;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class MeleeWeapon : BaseWeapon
{
    private Transform armPivot;

    [SerializeField]
    private float restAngle = 0f;
    [SerializeField]
    private float raisedAngle = -110f;
    [SerializeField]
    private float swingAngle = 70f;

    private float baseAngle;

    [SerializeField] float rotateLerpSpeed = 18f;

    private float chargeRatio;
    private float swingProgress;
    private bool swinging;


    private List<Collider2D> hitTargets = new List<Collider2D>();

    public override void Update()
    {
        base.Update();

        if (armPivot == null) return;


        if (!swinging)
        {
            TickChargePose();
        }
        else
        {
            TickSwing();
        }
    }

    public override void BeginCharge()
    {
        swinging = false;
        hitTargets.Clear();
    }

    public void SetChargeRatio(float ratio)
    {
        chargeRatio = Mathf.Clamp01(ratio);
    }

    public override void Use()
    {
        if (!CanUse()) return;

        swinging = true;
        swingProgress = 0f;

        ResetCooldown();
    }

    void TickChargePose()
    {
        float eased = Mathf.SmoothStep(0, 1, chargeRatio);
        float target = baseAngle + Mathf.Lerp(restAngle, raisedAngle, eased);

        float newAngle = Mathf.LerpAngle(
            armPivot.localEulerAngles.z,
            target,
            Time.deltaTime * rotateLerpSpeed
        );

        armPivot.localRotation = Quaternion.Euler(0, 0, newAngle);
    }

    void TickSwing()
    {
        swingProgress += Time.deltaTime * 6f;
        float t = Mathf.Clamp01(swingProgress);

        float smoothT = Mathf.SmoothStep(0, 1, t);

        float angle = baseAngle + Mathf.Lerp(raisedAngle, swingAngle, smoothT);

        float newAngle = Mathf.LerpAngle(
            armPivot.localEulerAngles.z,
            angle,
            Time.deltaTime * rotateLerpSpeed
        );

        armPivot.localRotation = Quaternion.Euler(0, 0, newAngle);

        if (t >= 1f)
        {
            swinging = false;
        }
    }


    public void SetArmPivot(Transform pivot)
    {
        armPivot = pivot;
    }

    public void SetBaseAngle(Vector2 forward)
    {
        baseAngle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
    }
}
