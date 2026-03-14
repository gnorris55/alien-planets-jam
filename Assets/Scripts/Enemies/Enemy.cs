using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{

    public enum EnemyState
    {
        noTarget,
        targetFound,
        targetInAttackRange
    }

    public event EventHandler OnHealthUpdated;
    public event EventHandler <OnEnemyDestroyedArgs>OnEnemyDestroyed;
    public class OnEnemyDestroyedArgs : EventArgs
    {
        public Enemy enemy;
    }


    [SerializeField] protected float speed = 0.2f;
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float attackFrequencyTime = 1f;
    [SerializeField] protected float damage;

    protected float attackFrequencyTimer;
    protected EnemyState currentState;
    protected PlanetObject planetObjectTarget;
   

    [SerializeField] private float maxHealth = 100;
    private float currentHealth;


    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void SetUp(float speed, Vector3 direction)
    {
        this.speed = speed;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        OnHealthUpdated?.Invoke(this, EventArgs.Empty);

        if (currentHealth <= 0) 
        {
            OnEnemyDestroyed?.Invoke(this, new OnEnemyDestroyedArgs { enemy = this});
            Destroy(gameObject);
        }
    }
    public void AddHealth(float healthAmount)
    {

    }

    public float GetCurrentHealth() 
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }
    public void SetHealth(float healthAmount)
    {
        this.currentHealth = healthAmount;
    }

    public void SetTarget(PlanetObject planetObject)
    {
        this.planetObjectTarget = planetObject;
        planetObjectTarget.OnPlanetObjectDestroyed += PlanetObjectTarget_OnPlanetObjectDestroyed;

    }

    private void PlanetObjectTarget_OnPlanetObjectDestroyed(object sender, PlanetObject.OnPlanetObjectDestroyedArgs e)
    {
        planetObjectTarget.OnPlanetObjectDestroyed -= PlanetObjectTarget_OnPlanetObjectDestroyed;
        planetObjectTarget = null;
        currentState = EnemyState.noTarget;
    }


    public bool HasTarget()
    {
        return planetObjectTarget != null;
    }
}
