using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance { get; private set; }


    public event EventHandler OnShootInputPressed;
    public event EventHandler OnShootInputReleased;
    public event EventHandler OnSwapWeaponInputPressed;
    public event EventHandler OnPlayerInteractPressed;
    public event EventHandler OnPlayerInteractReleased;
    public event EventHandler OnChangePlayerStatePressed;
 

    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference shootInput;
    [SerializeField] InputActionReference swapWeaponInput;
    [SerializeField] InputActionReference playerInteractInput;
    [SerializeField] InputActionReference changePlayerStateInput;

    //[SerializeField] InputActionReference shoot;


    private bool canShoot = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        shootInput.action.started += ShootInput_started;
        shootInput.action.canceled += ShootInput_canceled;
        swapWeaponInput.action.started += SwapWeaponInput_started;
        playerInteractInput.action.started += PlayerInput_started;
        playerInteractInput.action.canceled += PlayerInput_canceled;
        changePlayerStateInput.action.started += ChangePlayerStateInput_started;
    }

    private void PlayerInput_canceled(InputAction.CallbackContext obj)
    {
        OnPlayerInteractReleased?.Invoke(this, EventArgs.Empty);
    }

    private void ChangePlayerStateInput_started(InputAction.CallbackContext obj)
    {
        OnChangePlayerStatePressed?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerInput_started(InputAction.CallbackContext obj)
    {
        OnPlayerInteractPressed?.Invoke(this, EventArgs.Empty);
    }

    private void ShootInput_canceled(InputAction.CallbackContext obj)
    {
        OnShootInputReleased?.Invoke(this, EventArgs.Empty);
    }

    private void SwapWeaponInput_started(InputAction.CallbackContext obj)
    {
        OnSwapWeaponInputPressed?.Invoke(this, EventArgs.Empty);
    }

    private void ShootInput_started(InputAction.CallbackContext obj)
    {
        if (canShoot)
            OnShootInputPressed?.Invoke(this, EventArgs.Empty);
    }

    public void ToggleShootInput(bool canShoot)
    {
        this.canShoot = canShoot;
    }

    public Vector2 GetMovement()
    {
        return moveInput.action.ReadValue<Vector2>();
    }
}
