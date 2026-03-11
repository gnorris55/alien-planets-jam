using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{

    public event EventHandler OnHealthUpdated;

    public event EventHandler <OnEnemyDestroyedArgs>OnEnemyDestroyed;
    public class OnEnemyDestroyedArgs : EventArgs
    {
        public Enemy enemy;
    }

    [SerializeField] private float maxHealth = 100;
    [SerializeField] protected float speed = 0.2f;

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


    public float GetCurrentHealth() 
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
