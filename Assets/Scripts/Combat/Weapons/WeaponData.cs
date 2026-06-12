using UnityEngine;

public enum WeaponType
{
    Melee,
    Ranged
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]

public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;


    public float attackCooldown;

    public Vector3 weaponOffset;

    public float minKnockback;
    public float maxKnockback;

    public float minDamage;
    public float maxDamage;

    public float chargeSpeed;

}
