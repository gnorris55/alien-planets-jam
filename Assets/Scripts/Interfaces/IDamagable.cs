using System;
using UnityEngine;

public interface IDamagable
{
    public event EventHandler OnHealthUpdated;
    
    public void TakeDamage(float damageAmount);
    public void AddHealth(float healthAmount);
    public float GetCurrentHealth();
    public float GetMaxHealth();
    public void SetMaxHealth(float maxHealth);
    public void SetHealth(float healthAmount);

}
