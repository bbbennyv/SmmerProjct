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


    private bool isLeftCharging;
    private bool isRightCharging;

    private float leftCooldown;
    private float rightCooldown;
    [SerializeField]private float cooldown = 0.3f;

    private float leftCharge;
    private float rightCharge;

    [SerializeField] private FistController leftFist;
    [SerializeField] private FistController rightFist;

    [SerializeField] private ParticleSystem PunchParticles;
    [SerializeField] private ParticleSystem PunchParticlesCharged;
    private float charge;

    private Rigidbody2D rb;
    private PlayerController fighter;
    private EnemyScan tracker;

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

        if(isLeftCharging)
        {
            leftCharge = Mathf.Min(leftCharge + Time.deltaTime, maxChargeTime);
            leftFist.SetChargeRatio(leftCharge/ maxChargeTime);
        }

        if (isRightCharging)
        {
            rightCharge = Mathf.Min(rightCharge + Time.deltaTime, maxChargeTime);
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

        }
        if (hand == Hand.Right && rightCooldown <= 0)
        {
            isRightCharging = true;
            rightCharge = 0;
            rightFist.StartCharge();

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
        //Vector2 punchDir = getPunchDirection();
        //float selfImpulse = Mathf.Lerp(minPunchForce, maxPunchForce, chargeAmount) * 0.1f;
        //rb.AddForce(punchDir * selfImpulse, ForceMode2D.Impulse);
    }
    Vector2 TowardEnemy()
    {
        if (tracker != null && tracker.closestEnemy != null)
            return tracker.DirectionToEnemy;

        return fighter.transform.localScale.x >= 0 ? Vector2.right : Vector2.left;
    }
    Vector2 TowardEnemyWithBias()
    {
        float x = Mathf.Sign(TowardEnemy().x);
        return new Vector2(x * horizontalKnockback,  verticalKnockback).normalized;
    }
    void HandleHit(Collider2D other, float chargeAmount, Hand hand)
    {
        float knockback = Mathf.Lerp(minPunchForce, maxPunchForce, chargeAmount);

        Vector2 hitDir = TowardEnemyWithBias();

        Rigidbody2D targetRb = other.attachedRigidbody;
        if (targetRb != null)
        {
            targetRb.AddForce(hitDir * knockback, ForceMode2D.Impulse);
            

                Debug.Log(charge);
            if(charge < 0.5)
            {
                Instantiate(PunchParticles, targetRb.position, Quaternion.identity);
                CameraShake.Instance.ShakeCamera(5f, .1f);
            }
            else
            {
                Instantiate(PunchParticlesCharged, targetRb.position, Quaternion.identity);
                CameraShake.Instance.ShakeCamera(7f, .1f);

            }
        }
    }


}
