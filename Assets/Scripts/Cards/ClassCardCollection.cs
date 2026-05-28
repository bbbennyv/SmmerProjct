using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Class Card Collection")]
public class ClassCardCollection : ScriptableObject
{
   public PlayerClasses PlayerClass;
   public CardCollection ClassCards;
   public CardCollection NeutralCards;
}
