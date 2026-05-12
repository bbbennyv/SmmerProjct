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


    public int damage;
    public float attackCooldown;

}
