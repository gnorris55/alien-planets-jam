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
   

    private void Start()
    {
        currentHealth = maxHealth;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f));
        mySequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f));
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


    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Debug.Log("should destroy object");
            OnPlanetObjectDestroyed?.Invoke(this, new OnPlanetObjectDestroyedArgs { planetObject = this });
            Destroy(gameObject);
        }
    }

    public float GetCurrentHealth()
    {
        return maxHealth;
    }

    public float GetMaxHealth()
    {
        return currentHealth;
    }

    public float GetWidth()
    {
        return width;
    }
}
