using UnityEngine;

public abstract class RuntimeEffectSO : ScriptableObject
{
    public abstract RuntimeCardEffect CreateRuntimeEffect(GameObject user);

}
