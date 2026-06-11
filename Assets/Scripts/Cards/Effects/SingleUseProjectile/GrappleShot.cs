using UnityEngine;

public class GrappleShot : RuntimeCardEffect
{
    private float grappleSpeed;
    private bool isPulling = false;
    private Vector2 targetPosition;
    private Rigidbody2D ownerRb;

    public void Init(float speed)
    {
        grappleSpeed = speed;
    }

    public override void OnHitProjectile(ProjectileContext projCont, Collider2D other)
    {
        ownerRb = projCont.owner.GetComponent<Rigidbody2D>();

        targetPosition = other.transform.position;

        isPulling = true;
    }

    private void FixedUpdate()
    {
        if (!isPulling || ownerRb == null) return;

        Vector2 currentPos = ownerRb.position;
        Vector2 direction = (targetPosition - currentPos).normalized;
        float distance = Vector2.Distance(currentPos, targetPosition);

        //Vector2 movement = direction * grappleSpeed * Time.fixedDeltaTime;

        if (distance <= 2f)
        {
            isPulling = false;
            ConsumeUse();
            return;
        }
        ownerRb.AddForce(direction * grappleSpeed, ForceMode2D.Force);
    }

}
