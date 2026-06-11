using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/Runtime/PiercingShot")]
public class PiercingShotEffectSO : RuntimeEffectSO
{
    public override RuntimeCardEffect CreateRuntimeEffect(GameObject user)
    {
        var effect = user.AddComponent<PiercingShot>();
        return effect;
    }
}
