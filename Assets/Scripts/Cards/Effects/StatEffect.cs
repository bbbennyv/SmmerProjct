using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/StatEffect")]
public class StatEffect : CardEffect
{

    [SerializeField] private StatType stat;
    [SerializeField] private float amount;
    [SerializeField] private float duration;


    public override void Execute(GameObject user)
    {
        user.GetComponent<PlayerController>()?.ModifyStat(stat, amount, duration);
    }
}
