using UnityEngine;


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