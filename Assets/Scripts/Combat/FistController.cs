using System.Buffers.Text;
using Unity.VisualScripting;
using UnityEngine;

public class FistController : MonoBehaviour
{
    [SerializeField] private Hand side;

    //How far in front of player the fists rest
    [SerializeField] private float anchorForward = 0.42f;
    //Stagger between fists to prevent overlap
    [SerializeField] private float anchorLateral = 0.18f;
    //How far back fists charge
    [SerializeField] private float chargeDistance = 0.35f;
    //How far punches travel
    [SerializeField] private float punchForwardDistance = 0.85f;
    //Offset of fists sitting on player
    [SerializeField] private float verticalOffset = 0.05f;

    [SerializeField] private float returnSpeed = 18f;  
    [SerializeField] private float chargeSpeed = 10f;  
    [SerializeField] private float punchSpeed = 40f;   

    [SerializeField] private float punchDuration = 0.12f;

    [SerializeField] private float chargeLift = 0.25f;

    [SerializeField] public LayerMask hitLayers;

    public FistState State { get; private set; } = FistState.Idle;

    private Rigidbody2D rb;
    [SerializeField]
    private Transform ownerTransform;    
    private CircleCollider2D circleCollider;
    private EnemyScan tracker;

    private float punchTimer;
    private float chargeRatio;    
    private bool hitRegistered;

    private float lateralSign;

    private bool isFistFull = false;
    private BaseWeapon currentWeapon; 

    public System.Action<Collider2D, float> OnFistHit;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        tracker = GetComponentInParent<EnemyScan>();

        circleCollider.isTrigger = true;

        lateralSign = (side == Hand.Left) ? 1f : -1f;

        transform.position = GetWorldTarget();

    }

    void FixedUpdate()
    {
        Vector2 target = GetWorldTarget();

        float speed = GetCurrentSpeed();

        rb.MovePosition(Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime));

        if (State == FistState.Punching)
        {
            punchTimer -= Time.fixedDeltaTime;
            if (punchTimer <= 0f)
                SetState(FistState.Idle);
        }

        if(currentWeapon != null)
        {
            if(currentWeapon is MeleeWeapon melee)
            {
                circleCollider.enabled = false; 
                melee.SetChargeRatio(chargeRatio);
                melee.SetBaseAngle(TrackingForward());
            }
        }
    }

    public void StartCharge()
    {
        if (State == FistState.Punching) return;
        hitRegistered = false;
        SetState(FistState.Charging);
    }

    public void SetChargeRatio(float ratio)
    {
        chargeRatio = Mathf.Clamp01(ratio);
    }

    public float GetChargeRatio()
    {
        return chargeRatio;
    }

    public void ReleasePunch()
    {
        hitRegistered = false;

        if(currentWeapon != null)
        {
            if (chargeRatio > 0.9f)
            {
                punchTimer = punchDuration;
                SetState(FistState.Punching);
            }
            else
            {
                SetState(FistState.Idle);
            }

            return;
        }

        punchTimer = punchDuration;
        SetState(FistState.Punching);
    }

    public void ForceReturn()
    {
        SetState(FistState.Idle);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (State != FistState.Punching) return;
        if (circleCollider.enabled == false) return;
        if (hitRegistered) return;
        if (((1 << other.gameObject.layer) & hitLayers) == 0) return;

        if (other.transform.IsChildOf(ownerTransform) || other.transform == ownerTransform) return;

        hitRegistered = true;
        OnFistHit?.Invoke(other, chargeRatio);
        SetState(FistState.Idle);
    }


    void SetState(FistState newState)
    {
        State = newState;
    }

    float GetCurrentSpeed()
    {
        return State switch
        {
            FistState.Punching => punchSpeed,
            FistState.Charging => chargeSpeed,
            _ => returnSpeed,
        };
    }

    Vector2 GetWorldTarget()
    {
        Vector2 origin = ownerTransform.position;
        Vector2 forward = TrackingForward();
        Vector2 lateral = TrackingLateral();

        Vector2 offset = State switch
        {
            FistState.Charging => ChargeOffset(forward, lateral) + ChargeLift(forward),
            FistState.Punching => PunchOffset(forward, lateral),
            _ => AnchorOffset(forward, lateral),
        };

        return origin + offset + Vector2.up * verticalOffset;

    }

    Vector2 AnchorOffset(Vector2 fwd, Vector2 lat)
    {
        return fwd * anchorForward + lat * (anchorLateral * lateralSign);
    }

    Vector2 ChargeOffset(Vector2 fwd, Vector2 lat) 
    {
        float backDist = Mathf.Lerp(anchorForward, -chargeDistance, chargeRatio);
        return fwd * backDist + lat * (anchorLateral * lateralSign);
    }

    Vector2 ChargeLift(Vector2 forward)
    {
        Vector2 up = Vector2.up;

        float eased = Mathf.SmoothStep(0, 1, chargeRatio);

        return up * (eased * chargeLift);
    }

    Vector2 PunchOffset(Vector2 fwd, Vector2 lat)
    {
        return fwd * punchForwardDistance + lat * (anchorLateral * lateralSign * 0.5f);
    }

    Vector2 TrackingForward()
    {
        if(tracker != null && tracker.closestEnemy != null)
        {
            return tracker.DirectionToEnemy;
        }

        return ownerTransform.localScale.x >= 0 ? Vector2.right : Vector2.left;
    }

    Vector2 TrackingLateral()
    {
        Vector2 fwd = TrackingForward();
        return new Vector2(-fwd.y, fwd.x);
    }

    public void SetFistFull (bool fistFull)
    {
        isFistFull = fistFull;
    }

    public bool GetFistFull () 
    { 
        return isFistFull; 
    }

    public void SetWeapon(BaseWeapon weapon)
    {
        currentWeapon = weapon;
    }

    public BaseWeapon GetWeapon() { return currentWeapon; }
}

public enum FistState { Idle, Charging, Punching }

