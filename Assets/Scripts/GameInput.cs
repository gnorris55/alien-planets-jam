using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance { get; private set; }


    public event EventHandler OnShootInputPressed;
    public event EventHandler OnShootInputReleased;
    public event EventHandler OnSwapWeaponInputPressed;

    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference shootInput;
    [SerializeField] InputActionReference swapWeaponInput;

    //[SerializeField] InputActionReference shoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        shootInput.action.started += ShootInput_started;
        shootInput.action.canceled += ShootInput_canceled;
        swapWeaponInput.action.started += Action_started;
    }

    private void ShootInput_canceled(InputAction.CallbackContext obj)
    {
        OnShootInputReleased?.Invoke(this, EventArgs.Empty);
    }

    private void Action_started(InputAction.CallbackContext obj)
    {
        OnSwapWeaponInputPressed?.Invoke(this, EventArgs.Empty);
    }

    private void ShootInput_started(InputAction.CallbackContext obj)
    {
        OnShootInputPressed?.Invoke(this, EventArgs.Empty);
    }



    public Vector2 GetMovement()
    {
        return moveInput.action.ReadValue<Vector2>();
    }
}
