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

[CreateAssetMenu(menuName = "CardEffects/Runtime/PiercingShot")]
public class PiercingShotEffectSO : RuntimeEffectSO
{
    public override RuntimeCardEffect CreateRuntimeEffect(GameObject user)
    {
        var effect = user.AddComponent<PiercingShot>();
        return effect;
    }
}