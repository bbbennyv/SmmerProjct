using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/Runtime/ExplosiveShot")]
public class ExplosiveShotSO : RuntimeEffectSO
{
    [SerializeField] private float explosionRadius = 3f;

    [SerializeField] private float explosionKnockbackMultiplier = 1.5f;

    [SerializeField] private GameObject explosionEffect;

    public override RuntimeCardEffect CreateRuntimeEffect(GameObject user)
    {
        var effect = user.AddComponent<ExplosiveShot>();
        effect.Init(explosionRadius, explosionKnockbackMultiplier, explosionEffect);
        return effect;
    }
}

