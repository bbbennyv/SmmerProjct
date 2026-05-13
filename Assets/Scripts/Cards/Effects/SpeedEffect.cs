using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/Speed")]
public class SpeedEffect : CardEffect
{
    [SerializeField] private float speedBoost = 100f;
  
  public override void Execute(GameObject user)
  {
    //user.GetComponent<PlayerController>()?.ApplySpeedBoost(speedBoost);
    Debug.Log("Speeed");
  }
}
