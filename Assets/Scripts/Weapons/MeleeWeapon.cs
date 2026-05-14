using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

    [SerializeField]
    private float swingSpeed = 3f;

    private float baseAngle;

    [SerializeField] float rotateLerpSpeed = 18f;

    private float chargeRatio;
    private float swingProgress;
    private bool swinging;

    private PlayerController owner;
    private Transform ownerTransform;

    private Vector2 hitDirection;

    private bool hitRegistered = false;
    
    private float currentAngle;

    [SerializeField] public LayerMask hitLayers;


    private void Start()
    {
        owner = GetComponentInParent<PlayerController>();
        ownerTransform = owner.GetComponent<Transform>();
        
    }

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
        hitRegistered = false;
    }

    public void SetChargeRatio(float ratio)
    {
        chargeRatio = Mathf.Clamp01(ratio);
    }

    public override void Use()
    {
        if (!CanUse()) return;
        if (chargeRatio < 0.9f) return;
        if (swinging) return;

        swinging = true;
        swingProgress = 0f;

        float startAngle = baseAngle + (raisedAngle);
        currentAngle = startAngle;

        armPivot.localRotation = Quaternion.Euler(0, 0, FinalAngle(currentAngle));

        ResetCooldown();
    }

    void TickChargePose()
    {
        float eased = Mathf.SmoothStep(0, 1, chargeRatio);
        float target = baseAngle + Mathf.Lerp(restAngle, raisedAngle, eased);

        float newAngle = Mathf.LerpAngle(
            currentAngle,
            target,
            Time.deltaTime * rotateLerpSpeed
        );
        currentAngle = newAngle;
        armPivot.localRotation = Quaternion.Euler(0, 0, FinalAngle(currentAngle));
    }

    void TickSwing()
    {
        swingProgress += Time.deltaTime * swingSpeed;
        float t = Mathf.Clamp01(swingProgress);

        float smoothT = Mathf.SmoothStep(0, 1, t);

        float angle = baseAngle + Mathf.Lerp(raisedAngle, swingAngle, smoothT);

        float newAngle = Mathf.LerpAngle(
            currentAngle,
            angle,
            Time.deltaTime * rotateLerpSpeed
        );
        currentAngle = newAngle;
        armPivot.localRotation = Quaternion.Euler(0, 0, FinalAngle(currentAngle));

        if (t >= 1f)
        {
            swinging = false;
            chargeRatio = 0f;

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!swinging) return;
        if (hitRegistered) return;
        if (((1 << other.gameObject.layer) & hitLayers) == 0) return;
        if (other.transform.IsChildOf(ownerTransform) || other.transform == ownerTransform) return;

        hitRegistered = true;

        float knockback = Mathf.Lerp(weaponData.minKnockback, weaponData.maxKnockback, chargeRatio);
        int damage = (int)Mathf.Lerp(weaponData.minDamage, weaponData.maxDamage, chargeRatio);

        Rigidbody2D targetRb = other.attachedRigidbody;
        if (targetRb != null)
        {
            HealthSystem targetHealth = targetRb.GetComponent<HealthSystem>();
            targetHealth.TakeDamage(damage, knockback, hitDirection, targetRb, chargeRatio);
        }

    }

    public void SetArmPivot(Transform pivot)
    {
        armPivot = pivot;
    }

    public void SetBaseAngle(Vector2 forward)
    {
        hitDirection = forward;
        baseAngle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;

    }

    private float FinalAngle(float angle)
    {
        if(hitDirection.x < 0)
        {
            angle += 180f;
            angle = -angle;
        }

        return angle;
    }
}
