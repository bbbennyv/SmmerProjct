using System.Collections.Generic;
using UnityEngine;

public class CardEffectHandler : MonoBehaviour
{
    private List<RuntimeCardEffect> effects = new();

    public void Register(RuntimeCardEffect effect)
    {
        effects.Add(effect);
    }

    public void NotifyProjectileFired(ProjectileContext projCont)
    {
        foreach (var effect in effects) effect.OnProjectileFired(projCont);
    }

    public void NotifyProjectileHit(ProjectileContext projCont, Collider2D other)
    {
        foreach(var effect in effects) effect.OnHitProjectile(projCont, other);
    }
}
