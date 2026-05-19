using UnityEngine;

public class GunWeapon : RangedWeapon
{
    protected override Vector2 GetFireDirection()
    {
        return hitDirection.x >= 0 ? Vector2.right: Vector2.left;
    }
}
