using UnityEngine;

public class PiercingShot : RuntimeCardEffect
{
    public override void OnProjectileFired(ProjectileContext projCont)
    {
        var projectile = projCont.prefab.GetComponent<Projectile>();
        if(projectile != null)
        {
            projectile.setIsPiercing(true);
        }

        ConsumeUse();

    }
}

