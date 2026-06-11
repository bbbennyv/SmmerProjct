using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/Runtime/SuperChargedShot")]
public class SuperChargedShotEffectSO : RuntimeEffectSO
{
    [SerializeField] private float chargeMultiplier = 2f;
    public override RuntimeCardEffect CreateRuntimeEffect(GameObject user)
    {
        var effect = user.AddComponent<SuperChargedShot>();
        effect.SetChargeMultiplier(chargeMultiplier);
        return effect;
    }
}
