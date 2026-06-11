using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/Runtime/TripleShot")]
public class TripleShotEffectSO : RuntimeEffectSO
{
    public override RuntimeCardEffect CreateRuntimeEffect(GameObject user)
    {
        var effect = user.AddComponent<TripleShot>();

        return effect;
    }
}
