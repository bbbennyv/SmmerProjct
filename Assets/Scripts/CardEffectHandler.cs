using System.Collections.Generic;
using UnityEngine;

public class CardEffectHandler : MonoBehaviour
{
    private List<RuntimeCardEffect> effects = new();

    public void Register(RuntimeCardEffect effect)
    {
        if(!effects.Contains(effect))
        {
            effects.Add(effect);
        }
    }

    public void NotifyProjectileFired(ProjectileContext projCont)
    {
        effects.RemoveAll(e => e == null);

        foreach (var effect in effects) effect.OnProjectileFired(projCont);
    }

    public void NotifyProjectileHit(ProjectileContext projCont, Collider2D other)
    {
        effects.RemoveAll(e => e == null);

        foreach(var effect in effects) effect.OnHitProjectile(projCont, other);
    }
}
