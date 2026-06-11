using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class UINavigation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isHorizontal = false;
    [SerializeField] private float navCooldown = 0.2f;
    [SerializeField] private List<UIOption> options = new List<UIOption>();

    private List<Gamepad> gamepads = new List<Gamepad>();
    private int currentIndex = 0;
    private float cooldownTimer = 0f;
    private bool active = false;

    public void AddGamepad(Gamepad gamepad)
    {
        gamepads.Add(gamepad);
        if (!active)
        {
            active = true;
        }
    }

    private void OnEnable()
    {
        if (active)
            UpdateSelection();
    }

    public void InitializeWithPlayerInput(PlayerInput input)
    {
        var gamepad = input.devices.OfType<Gamepad>().FirstOrDefault();
        if (gamepad != null)
            AddGamepad(gamepad);
    }

    public void Initialize(List<UIOption> options)
    {
        this.options = options;
    }

    private void Update()
    {
        if (!active  || !GameManager.Instance.IsUpgrade || gamepads.Count == 0) return;

        cooldownTimer -= Time.deltaTime;

        foreach (var gamepad in gamepads)
        {
            if (cooldownTimer <= 0f)
            {
                float axis = isHorizontal ? gamepad.leftStick.ReadValue().x : gamepad.leftStick.ReadValue().y;

                if (axis > 0.5f)
                {
                    if (isHorizontal)
                    {
                        Move(1);
                    }
                    else
                    {
                        Move(-1);
                    }
                        cooldownTimer = navCooldown;
                    break;
                }
                else if (axis < -0.5f)
                {
                    if (isHorizontal)
                    {
                        Move(-1);
                    }
                    else
                    {
                        Move(1);
                    }
                    cooldownTimer = navCooldown;
                    break;
                }
            }

            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                options[currentIndex].OnConfirm?.Invoke();
                break;
            }

            if (gamepad.buttonEast.wasPressedThisFrame)
            {
                options[currentIndex].OnCancel?.Invoke();
                break;
            }
        }
    }

    private void Move(int direction)
    {
        options[currentIndex].SetHighlighted(false);
        currentIndex = Mathf.Clamp(currentIndex + direction, 0, options.Count - 1);
        options[currentIndex].SetHighlighted(true);
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < options.Count; i++)
        {
            options[i].SetHighlighted(i == currentIndex);
        }
    }
    public void SetActive(bool Active)
    {
        this.active = Active;
    }
}
