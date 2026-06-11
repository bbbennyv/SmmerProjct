using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MeleeWeapon : BaseWeapon
{
    [SerializeField]
    private float restAngle = 0f;
    [SerializeField]
    private float raisedAngle = -110f;
    [SerializeField]
    private float swingAngle = 70f;


    [SerializeField] private float swingDuration = 0.18f;

    [SerializeField] private AnimationCurve swingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private float poseSmoothing = 8f;

    [SerializeField] float disarmChance = 0.5f;

    private Transform ownerTransform;

    private enum SwingState { Idle, Charging, Swinging, Returning }
    private SwingState state = SwingState.Idle;


    private float currentAngle;
    private float lockedBaseAngle;
    private float swingTimer;

    private bool hitRegistered = false;
    [SerializeField] public LayerMask hitLayers;

    private void Start()
    {
        PlayerController owner = GetComponentInParent<PlayerController>();
        ownerTransform = owner.GetComponent<Transform>();

        currentAngle = ComputeBaseAngle() + restAngle;
    }

    public override void Update()
    {
        base.Update();

        if (armPivot == null) return;

        switch (state)
        {
            case SwingState.Idle: TickIdle(); break;
            case SwingState.Charging: TickChargePose(); break;
            case SwingState.Swinging: TickSwing(); break;
            case SwingState.Returning: TickReturnToRest(); break;
        }
        
        armPivot.localRotation = Quaternion.Euler(0, 0, FinalAngle(currentAngle));
    }

    public override void BeginCharge()
    {
        if(state == SwingState.Swinging) return;
        state = SwingState.Charging;
        hitRegistered = false;
    }

    public override void Use()
    {
        if (!CanUse()) return;
        if (state == SwingState.Swinging) return;

        lockedBaseAngle = ComputeBaseAngle();
        currentAngle = lockedBaseAngle + raisedAngle;
        
        state = SwingState.Swinging;
        swingTimer = 0f;
        hitRegistered = false;
        ResetCooldown();
    }

    void TickIdle()
    {
        SmoothToward(ComputeBaseAngle() + restAngle);
    }

    void TickChargePose()
    {
        float eased = Mathf.SmoothStep(0, 0.5f, chargeRatio);
        float target = ComputeBaseAngle() + Mathf.Lerp(restAngle, raisedAngle, eased);

        SmoothToward(target);
    }

    void TickSwing()
    {
        swingTimer += Time.deltaTime;
        float t = Mathf.Clamp01(swingTimer / swingDuration);

        float curveT = swingCurve.Evaluate(t);
        currentAngle = lockedBaseAngle + Mathf.Lerp(raisedAngle, swingAngle, curveT);

        if (t >= 1f)
        {
            chargeRatio = 0f;
            state = SwingState.Returning;

        }
    }

    void TickReturnToRest()
    {
        float target = ComputeBaseAngle() + restAngle;
        SmoothToward(target);

        if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, target)) < 1f)
        {
            currentAngle = target;
            state = SwingState.Idle;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (state != SwingState.Swinging) return;
        if (hitRegistered) return;
        if (((1 << other.gameObject.layer) & hitLayers) == 0) return;
        if (other.transform.IsChildOf(ownerTransform) || other.transform == ownerTransform) return;

        hitRegistered = true;

        float knockback = Mathf.Lerp(weaponData.minKnockback, weaponData.maxKnockback, chargeRatio);
        int damage = (int)Mathf.Lerp(weaponData.minDamage, weaponData.maxDamage, chargeRatio);

        Rigidbody2D targetRb = other.attachedRigidbody;
        if (targetRb != null)
        {
            if (chargeRatio >= 0.9f)
            {
                float random = Random.value;
                if(random <= disarmChance)
                {
                    PlayerController player = targetRb.GetComponent<PlayerController>();
                    player.RemoveAllWeapons();
                }

            }


            HealthSystem targetHealth = targetRb.GetComponent<HealthSystem>();
            targetHealth.TakeDamage(damage, knockback, hitDirection, targetRb, chargeRatio);
        }

    }

    public override void SetBaseAngle(Vector2 forward)
    {
        base.SetBaseAngle(forward);

    }

    public override bool CanCharge()
    {
        return true;
    }

    public override bool canStunOutOfHand()
    {
        return true;
    }

    float ComputeBaseAngle()
    {
        return baseAngleDegrees;
    }
    void SmoothToward(float target)
    {
        currentAngle = Mathf.LerpAngle(currentAngle, target, Time.deltaTime * poseSmoothing);
    }

}
