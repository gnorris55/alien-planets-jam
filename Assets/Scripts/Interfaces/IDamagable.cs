using System;
using UnityEngine;

public interface IDamagable
{
    public event EventHandler OnHealthUpdated;
    
    public void TakeDamage(float damageAmount);
    public float GetCurrentHealth();
    public float GetMaxHealth();
}
