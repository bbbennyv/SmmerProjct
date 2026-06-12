using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class PunchSystem : MonoBehaviour
{
    [SerializeField] private float maxChargeTime = 1.2f;

    [SerializeField] private float minPunchForce = 5.0f;
    [SerializeField] private float maxPunchForce = 20.0f;
    [SerializeField] private float verticalKnockback = 0.3f;
    [SerializeField] private float horizontalKnockback = 0.01f;
    [SerializeField] private int minAttackDamage = 10;
    [SerializeField] private int maxAttackDamage = 20;
    [SerializeField] float disarmChance = 0.5f;

    [SerializeField] private bool allowOvercharge = true;
    [SerializeField] private float overchargeMultiplier = 2f;

    private bool isLeftCharging;
    private bool isRightCharging;

    private float leftCooldown;
    private float rightCooldown;
    [SerializeField]private float cooldown = 0.3f;

    private float leftCharge;
    private float rightCharge;

    [SerializeField] private FistController leftFist;
    [SerializeField] private FistController rightFist;

    private float charge;

    private Rigidbody2D rb;
    private PlayerController fighter;
    private EnemyScan tracker;

    private float leftChargeSpeedMult = 1f;
    private float rightChargeSpeedMult = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fighter = GetComponent<PlayerController>();
        tracker = GetComponent<EnemyScan>();

        if (leftFist) leftFist.OnFistHit += (col, charge) => HandleHit(col, charge, Hand.Left);
        if (rightFist) rightFist.OnFistHit += (col, charge) => HandleHit(col, charge, Hand.Right);
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        leftCooldown = Mathf.Max(0, leftCooldown - Time.deltaTime);
        rightCooldown = Mathf.Max(0, rightCooldown - Time.deltaTime);

        float leftMax = maxChargeTime * (allowOvercharge ? overchargeMultiplier : 1f);
        float rightMax = maxChargeTime * (allowOvercharge ? overchargeMultiplier : 1f);

        if (isLeftCharging)
        {
            leftCharge = Mathf.Min(leftCharge + Time.deltaTime * leftChargeSpeedMult, leftMax);
            leftFist.SetChargeRatio(leftCharge/ maxChargeTime);
        }

        if (isRightCharging)
        {
            rightCharge = Mathf.Min(rightCharge + Time.deltaTime * rightChargeSpeedMult, rightMax);
            rightFist.SetChargeRatio(rightCharge/ maxChargeTime);
        }
        
    }

    public void ChargePunch(Hand hand)
    {
        if (hand == Hand.Left && leftCooldown <= 0)
        {
            isLeftCharging = true;
            leftCharge = 0;
            leftFist.StartCharge();
            UpdateChargeSpeed(Hand.Left);

        }
        if (hand == Hand.Right && rightCooldown <= 0)
        {
            isRightCharging = true;
            rightCharge = 0;
            rightFist.StartCharge();
            UpdateChargeSpeed(Hand.Right);

        }

    }

    public void ReleaseCharge(Hand hand)
    {
        if(hand == Hand.Left && isLeftCharging)
        {
            FirePunch(leftCharge / maxChargeTime, leftFist);
            leftCharge = 0;
            isLeftCharging = false;
            leftCooldown = cooldown;
        }
        if(hand == Hand.Right && isRightCharging)
        {
            FirePunch(rightCharge / maxChargeTime, rightFist);
            rightCharge = 0;
            isRightCharging = false;
            rightCooldown = cooldown;
        }
    }

    private void FirePunch(float chargeAmount, FistController fist)
    {
        if (fist == null) return;

        fist.SetChargeRatio(chargeAmount);
        fist.ReleasePunch();
        charge = fist.GetChargeRatio();

    }

    private void UpdateChargeSpeed(Hand hand)
    {
        FistController fist = hand == Hand.Left ? leftFist : rightFist;
        BaseWeapon weapon = fist?.GetWeapon();

        if(weapon == null)
        {
            if(hand == Hand.Left) leftChargeSpeedMult = 1f;
            else rightChargeSpeedMult = 1f;
            return;
        }

        float chargeSpeedMult = weapon.GetChargeSpeed();

        if (hand == Hand.Left)
        {
            leftChargeSpeedMult = chargeSpeedMult;
        }
        else
        {
            rightChargeSpeedMult = chargeSpeedMult;
        }

    }

    Vector2 TowardEnemy()
    {
        if (tracker != null && tracker.closestEnemy != null)
            return tracker.DirectionToEnemy;

        return fighter.transform.localScale.x >= 0 ? Vector2.right : Vector2.left;
    }

    //Vector2 TowardEnemyWithBias()
    //{
    //    float x = Mathf.Sign(TowardEnemy().x);
    //    return new Vector2(x * horizontalKnockback,  verticalKnockback).normalized;
    //}
    void HandleHit(Collider2D other, float chargeAmount, Hand hand)
    {
        float knockback = Mathf.Lerp(minPunchForce, maxPunchForce, chargeAmount);
        int damage = (int)Mathf.Lerp(minAttackDamage, maxAttackDamage, chargeAmount);

        Vector2 hitDir = TowardEnemy();

        Rigidbody2D targetRb = other.attachedRigidbody;
        if (targetRb != null)
        {
            if (charge >= 0.9f)
            {
                float random = Random.value;
                if (random <= disarmChance)
                {
                    PlayerController player = targetRb.GetComponent<PlayerController>();
                    player.RemoveAllWeapons();
                }

            }
            HealthSystem targetHealth = targetRb.GetComponent<HealthSystem>();
            targetHealth.TakeDamage(damage, knockback, hitDir, targetRb, chargeAmount);
        }
    }

    public FistController GetLeftFist()
    {
        return leftFist;
    }

    public FistController GetRightFist()
    {
        return rightFist;
    }
}
