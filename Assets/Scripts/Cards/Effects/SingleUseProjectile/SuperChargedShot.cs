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

        Destroy(this);
    }
    public void SetChargeMultiplier(float multiplier)
    {
        chargeMultiplier = multiplier;
    }
}


                                                                              