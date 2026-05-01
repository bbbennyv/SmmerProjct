using UnityEngine;
using UnityEngine.XR;

public class PunchSystem : MonoBehaviour
{
    [SerializeField] private float maxChargeTime = 1.2f;

    [SerializeField] private float minPunchForce = 5.0f;
    [SerializeField] private float maxPunchForce = 20.0f;
    [SerializeField] private float upwardsAngle = 0.3f;

    private bool isLeftCharging;
    private bool isRightCharging;

    private float leftCooldown;
    private float rightCooldown;
    const float cooldown = 0.3f;

    private float leftCharge;
    private float rightCharge;

    [SerializeField] private FistController leftFist;
    [SerializeField] private FistController rightFist;

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
        Vector2 dir = TowardEnemy();
        return new Vector2(dir.x, dir.y + upwardsAngle).normalized;
    }
    void HandleHit(Collider2D other, float chargeAmount, Hand hand)
    {
        float knockback = Mathf.Lerp(minPunchForce, maxPunchForce, chargeAmount);

        Vector2 hitDir = TowardEnemyWithBias();

        Rigidbody2D targetRb = other.attachedRigidbody;
        if (targetRb != null)
        {
            targetRb.AddForce(hitDir * knockback, ForceMode2D.Impulse);
        }
    }


}
