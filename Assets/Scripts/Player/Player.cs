using System;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public enum PlayerStates
    {
        combat,
        building
    }


    public static Player Instance { get; private set; }

    public event EventHandler<OnHealthUpdatedArgs> OnHealthUpdated;
    public class OnHealthUpdatedArgs: EventArgs
    {
        public float updatedHealth;
        public float maxHealth;
    }

    public event EventHandler<OnPlayerStateChangedArgs> OnPlayerStateChanged;
    public class OnPlayerStateChangedArgs : EventArgs
    {
        public PlayerStates playerState;
    }


    public event EventHandler<OnFuelUpdatedArgs> OnFuelUpdated;
    public class OnFuelUpdatedArgs : EventArgs
    {
        public float updatedFuel;
        public float maxFuel;
    }


    [SerializeField] private float maxHealth;
    [SerializeField] private float maxFuelAmount;

    private float health;
    private float fuelAmount;
    private PlayerStates currentPlayerState = PlayerStates.combat;


    private void Awake()
    {
        Instance = this;
        health = maxHealth;
        fuelAmount = maxFuelAmount;
    }

    private void Start()
    {
        OnHealthUpdated?.Invoke(this, new OnHealthUpdatedArgs { maxHealth = maxHealth, updatedHealth = health });
        OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedArgs { playerState = currentPlayerState });
        GameInput.Instance.OnChangePlayerStatePressed += GameInput_OnChangePlayerStatePressed;
    }

    private void GameInput_OnChangePlayerStatePressed(object sender, EventArgs e)
    {

        if (gameObject.GetComponent<PlayerMovement>().GetCurrentPlanet() != null)
        {
            int numPlayerStates = Enum.GetNames(typeof(PlayerStates)).Length;
            int nextState = ((int)currentPlayerState + 1) % numPlayerStates;

            currentPlayerState = (PlayerStates)nextState;
            OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedArgs { playerState = currentPlayerState });
            Debug.Log(currentPlayerState.ToString());
        }
    }

    private void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0 )
        {
            Debug.Log("Player has died");
        }

    }

    public void SetState(PlayerStates playerState)
    {
        currentPlayerState = playerState;
        OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedArgs { playerState = currentPlayerState });
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetFuelAmount()
    {
        return fuelAmount;
    }

    public float GetMaxFuelAmount()
    {
        return maxFuelAmount;
    }


}
