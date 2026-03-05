using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance { get; private set; }


    public event EventHandler OnShootInputPressed;

    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference shootInput;

    //[SerializeField] InputActionReference shoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        shootInput.action.started += ShootInput_started;
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
