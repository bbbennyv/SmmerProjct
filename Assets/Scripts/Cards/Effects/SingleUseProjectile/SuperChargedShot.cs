using UnityEngine;

public class SuperChargedShot : RuntimeCardEffect
{
    private float chargeMultiplier; 
    public override void OnProjectileFired(ProjectileContext projCont)
    {
        var projectile = projCont.prefab.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.MultiplySpeedMultiplier(chargeMultiplier);
        }

        ConsumeUse();
    }
    public void SetChargeMultiplier(float multiplier)
    {
        chargeMultiplier = multiplier;
    }
}

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
                                                                              