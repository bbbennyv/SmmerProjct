using UnityEngine;

public class PlayerHandUI : MonoBehaviour
{
    public static PlayerHandUI[] All {get; private set; } = new PlayerHandUI[4];
    [SerializeField] private int _playerIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 private void Awake()
 {
    All[_playerIndex] = this;
 }
}
