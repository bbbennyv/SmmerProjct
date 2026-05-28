using System.Collections.Generic;
using UnityEngine;

public class CardEffectHandler : MonoBehaviour
{
    private List<RuntimeCardEffect> effects = new();

    public void Register(RuntimeCardEffect effect)
    {
        effects.Add(effect);
    }

    public void NotifyProjectileFired(RangedWeapon weapon, Projectile proj)
    {
        foreach (var effect in effects) effect.OnProjectileFired(weapon, proj);
    }
}
