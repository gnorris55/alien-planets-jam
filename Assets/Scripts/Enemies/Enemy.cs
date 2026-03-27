using NUnit.Framework;
using System;
using TMPro.EditorUtilities;
using UnityEngine;
using System.Collections.Generic;

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
    protected Planet homePlanet;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float oilDropAmount = 20f;
    [SerializeField] private ItemVisualMovement oilGlobVisual;
    [SerializeField] private ParticleSystem DieParticleGoo;
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
        Instantiate(DieParticleGoo, transform.position, Quaternion.identity);

        if (currentHealth <= 0) 
        {
            OnEnemyDestroyed?.Invoke(this, new OnEnemyDestroyedArgs { enemy = this});
            // transfer oil to player
            if (Player.Instance.GetCurrentPlanet() == homePlanet)
            {
                float leftOverOil = Player.Instance.AddOil(oilDropAmount);
                if (leftOverOil > 0)
                {
                    List<OilStorage> oilStorageDevices = homePlanet.GetSpecificPlanetObject<OilStorage>();
                    foreach (OilStorage oilStorage in oilStorageDevices)
                    {

                        leftOverOil = oilStorage.AddOil(leftOverOil);

                        if (leftOverOil == 0)
                        {
                            ItemVisualMovement oilGlobInstance = Instantiate(oilGlobVisual, transform.position, Quaternion.identity);
                            oilGlobInstance.SetUp(transform.position, oilStorage.transform.position);
                            break;
                        } 

                    }

                }
                else
                {
                    ItemVisualMovement oilGlobInstance = Instantiate(oilGlobVisual, transform.position, Quaternion.identity);
                    oilGlobInstance.SetUp(transform.position, Player.Instance.transform.position);
                }

            }
            Instantiate(DieParticleGoo, transform.position, Quaternion.identity);
            Instantiate(DieParticleGoo, transform.position, Quaternion.identity);
            Instantiate(DieParticleGoo, transform.position, Quaternion.identity);
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
