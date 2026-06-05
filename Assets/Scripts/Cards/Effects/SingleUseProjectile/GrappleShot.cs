using UnityEngine;

public class GrappleShot : RuntimeCardEffect
{
    private float grappleSpeed;

    public void Init(float speed)
    {
        grappleSpeed = speed;
    }

    public override void OnHitProjectile(ProjectileContext projCont, Collider2D other)
    {
        Vector2 launchDirection = projCont.direction.normalized;

        Rigidbody2D rigidbody2D = projCont.owner.GetComponent<Rigidbody2D>();

        rigidbody2D.AddForceX(launchDirection.x * grappleSpeed, ForceMode2D.Impulse);

        base.OnHitProjectile(projCont, other);
    }
}

[CreateAssetMenu(menuName = "CardEffects/Runtime/GrappleShot")]
public class GrappleShotEffectSO : RuntimeEffectSO
{
    [SerializeField]
    private float grappleSpeed = 200.0f;
    public override RuntimeCardEffect CreateRuntimeEffect(GameObject user)
    {
        var effect = user.AddComponent<GrappleShot>();
        effect.Init(grappleSpeed);

        return effect;
    }
}