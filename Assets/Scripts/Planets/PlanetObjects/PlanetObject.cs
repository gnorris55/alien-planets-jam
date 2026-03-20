using DG.Tweening;
using System;
using UnityEngine;

public class PlanetObject: MonoBehaviour, IDamagable
{


    public event EventHandler OnHealthUpdated;
    public event EventHandler<OnPlanetObjectDestroyedArgs> OnPlanetObjectDestroyed;
    public class OnPlanetObjectDestroyedArgs : EventArgs
    {
        public PlanetObject planetObject;
    }

    [SerializeField] private float width;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] protected bool isInteractable = true;

    protected Planet homePlanet;

    private float currentHealth;

    public virtual void SetHomePlanet(Planet planet)
    {
        homePlanet = planet;
    }
   

    protected virtual void Start()
    {

        StatsManager.Instance.OnGameObjectStatsUpdated += StatsManager_OnGameObjectStatsUpdated;
        
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f));
        mySequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f));
    }

    protected virtual void StatsManager_OnGameObjectStatsUpdated(object sender, StatsManager.OnGameObjectStatsUpgradedArgs e)
    {
        //print("should update " + e.objectType.ToString());
    }

    public virtual void Interact(Player player)
    {
        
    }
    public virtual void InteractStopped()
    {

    }

    public virtual void ShowInteractable()
    {
        PlayerHints.Instance.DisplayHint("PRESS E TO INTERACT");
    }
    public virtual void HideInteractable()
    {
        PlayerHints.Instance.HideHint();
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }


    public virtual void TakeDamage(float damageAmount)
    {
        DamageStructure(damageAmount);
        if (IsDestroyed())
        {
            DestoryPlanetObject();
        }
    }
    protected void DamageStructure(float damageAmount)
    {
        currentHealth -= damageAmount;
        OnHealthUpdated?.Invoke(this, EventArgs.Empty);
    }

    protected bool IsDestroyed()
    {
        return currentHealth <= 0;
    }

    protected void DestoryPlanetObject()
    {
        OnPlanetObjectDestroyed?.Invoke(this, new OnPlanetObjectDestroyedArgs { planetObject = this });
        Destroy(gameObject);

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
        currentHealth = healthAmount;
        OnHealthUpdated?.Invoke(this, EventArgs.Empty);
        //this.OnHealthUpdated.Invoke(this, EventArgs.Empty);
    }

    public float GetWidth()
    {
        return width;
    }
}
