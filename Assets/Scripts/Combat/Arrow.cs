using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private float gravityScale = 1.5f;

    [SerializeField] private float rotateSpeed = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        rb.gravityScale = gravityScale;

        float launchSpeed = speed * speedMultiplier;

        rb.linearVelocity = direction * launchSpeed;

        RotateInstantly();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (hitRegistered)
        {
            Destroy(gameObject);
        }

        Rotate();
    }

    private void Rotate()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.sqrMagnitude < 0.01f)
            return;

        float angle =
            Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        Quaternion targetRot =
            Quaternion.Euler(0f, 0f, angle - 90f);

        transform.rotation =
            Quaternion.Lerp(
                transform.rotation,
                targetRot,
                Time.fixedDeltaTime * rotateSpeed
            );
    }

    private void RotateInstantly()
    {
        Vector2 velocity = rb.linearVelocity;

        float angle =
            Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
