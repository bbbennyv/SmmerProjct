using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class UIManager : MonoBehaviour
{
    [SerializeField] private UINavigation navigation;

    private HashSet<Gamepad> registeredGamepads = new HashSet<Gamepad>();

    private void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (!registeredGamepads.Contains(gamepad))
            {
                registeredGamepads.Add(gamepad);
                navigation.AddGamepad(gamepad);
            }
        }
    }
}
