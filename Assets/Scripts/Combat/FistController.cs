using UnityEngine;

public class FistController : MonoBehaviour
{
    [SerializeField] private Hand side;

    [SerializeField] private Vector2 anchorOffset = new Vector2(1.5f, 0f);
    [SerializeField] private Vector2 chargeOffset = new Vector2(0.1f, 0f);
    [SerializeField] private Vector2 punchOffset = new Vector2(0.9f, 0.05f);

    [SerializeField] private float returnSpeed = 18f;  
    [SerializeField] private float chargeSpeed = 10f;  
    [SerializeField] private float punchSpeed = 40f;   
    [SerializeField] private float punchDuration = 0.12f; 

    [SerializeField] public LayerMask hitLayers;

    public FistState State { get; private set; } = FistState.Idle;

    private Rigidbody2D rb;
    private Transform ownerTransform;    
    private CircleCollider2D circleCollider;

    private float punchTimer;
    private float chargeRatio;    
    private bool hitRegistered;

    public System.Action<Collider2D, float> OnFistHit;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        ownerTransform = transform.parent;
        circleCollider.isTrigger = true;

        transform.localPosition = anchorOffset;
    }

    void FixedUpdate()
    {
        Vector2 targetLocal = GetTargetLocalPosition();

        Vector2 worldTarget = ownerTransform.TransformPoint(targetLocal);
        float speed = GetCurrentSpeed();
        Vector2 newPos = Vector2.MoveTowards(rb.position, worldTarget, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (State == FistState.Punching)
        {
            punchTimer -= Time.fixedDeltaTime;
            if (punchTimer <= 0f)
                SetState(FistState.Idle);
        }
    }

    public void StartCharge()
    {
        if (State == FistState.Punching) return;
        SetState(FistState.Charging);
        hitRegistered = false;
    }

    public void SetChargeRatio(float ratio)
    {
        chargeRatio = Mathf.Clamp01(ratio);
    }

    public void ReleasePunch()
    {
        SetState(FistState.Punching);
        punchTimer = punchDuration;
        hitRegistered = false;
    }

    public void ForceReturn()
    {
        SetState(FistState.Idle);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (State != FistState.Punching) return;
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

    Vector2 GetTargetLocalPosition()
    {
        return State switch
        {
            FistState.Charging => Vector2.Lerp(anchorOffset, chargeOffset, chargeRatio),
            FistState.Punching => punchOffset,
            _ => anchorOffset,   
        };
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

}

public enum FistState { Idle, Charging, Punching }

