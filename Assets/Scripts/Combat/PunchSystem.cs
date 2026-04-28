using UnityEngine;
using UnityEngine.XR;

public class PunchSystem : MonoBehaviour
{
    [SerializeField]
    private float maxChargeTime = 1.2f;

    [SerializeField]
    private float minPunchForce = 5.0f;
    [SerializeField]
    private float maxPunchForce = 20.0f;
    [SerializeField]
    public float fistRadius = 0.18f;
    [SerializeField]
    public float punchReach = 0.55f;

    [SerializeField]
    private LayerMask hitLayers;



    private bool isLeftCharging;
    private bool isRightCharging;

    private float leftCooldown;
    private float rightCooldown;
    const float cooldown = 0.3f;

    private float leftCharge;
    private float rightCharge;

    private Transform leftHandBone;
    private Transform rightHandBone;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftCooldown = Mathf.Max(0, leftCooldown - Time.deltaTime);
        rightCooldown = Mathf.Max(0, rightCooldown - Time.deltaTime);

        if(isLeftCharging)
        {
            leftCharge = Mathf.Min(leftCharge + Time.deltaTime, maxChargeTime);
        }

        if (isRightCharging)
        {
            rightCharge = Mathf.Min(rightCharge + Time.deltaTime, maxChargeTime);
        }
    }

    public void ChargePunch(Hand hand)
    {
        if (hand == Hand.Left && leftCooldown <= 0) isLeftCharging = true;
        if (hand == Hand.Right && rightCooldown <= 0) isRightCharging = true;

    }

    public void ReleaseCharge(Hand hand)
    {
        if(hand == Hand.Left && isLeftCharging)
        {
            FirePunch(Hand.Left, leftCharge / maxChargeTime);
            leftCharge = 0;
            isLeftCharging = false;
            leftCooldown = cooldown;
        }
        if(hand == Hand.Right && isRightCharging)
        {
            FirePunch(Hand.Right, rightCharge / maxChargeTime);
            rightCharge = 0;
            isRightCharging = false;
            rightCooldown = cooldown;
        }
    }

    private void FirePunch(Hand hand, float ChargeAmount)
    {
        Transform handBone = hand == Hand.Left ? leftHandBone : rightHandBone;

        float force = Mathf.Lerp(minPunchForce, maxPunchForce, ChargeAmount);

        Vector2 facing = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 punchDirection = (facing + Vector2.up).normalized;

        Vector2 origin = handBone.position;

        RaycastHit2D hit = Physics2D.CircleCast(origin, fistRadius, punchDirection, punchReach, hitLayers);

        if( hit)
        {
            Rigidbody2D targetRb = hit.rigidbody;

            if (targetRb != null) {

                targetRb.AddForce(punchDirection * force, ForceMode2D.Impulse);

                PlayerController targetPlayer = targetRb.GetComponent<PlayerController>();
                if (targetPlayer != null) { 
                
                    

                }

            }

        }
    }

}
