using System;
using Unity.Hierarchy;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }

    public event EventHandler<OnHealthUpdatedArgs> OnHealthUpdated;
    public class OnHealthUpdatedArgs: EventArgs
    {
        public float updatedHealth;
        public float maxHealth;
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


    private void Awake()
    {
        Instance = this;
        health = maxHealth;
        fuelAmount = maxFuelAmount;
    }

    private void Start()
    {
        OnHealthUpdated?.Invoke(this, new OnHealthUpdatedArgs { maxHealth = maxHealth, updatedHealth = health });
    }

    private void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0 )
        {
            Debug.Log("Player has died");
        }

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
