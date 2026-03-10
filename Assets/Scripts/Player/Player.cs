using System;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Player : MonoBehaviour
{

    public enum PlayerStates
    {
        combat,
        building
    }

    [SerializeField] private float maxHealth;
    [SerializeField] private float maxOilAmount;
    [SerializeField] private float fuelBurnRate;
    [SerializeField] private LayerMask interactableObjectsLayer;



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


    public event EventHandler<OnOilUpdatedArgs> OnOilUpdated;
    public class OnOilUpdatedArgs : EventArgs
    {
        public float updatedFuel;
        public float maxFuel;
    }



    private float health;
    private float currentOilAmount;
    private PlanetObject currentInteractablePlanetObject;
    private PlayerStates currentPlayerState = PlayerStates.combat;

    private void Awake()
    {
        Instance = this;
        health = maxHealth;
        currentOilAmount = maxOilAmount / 3f;
    }

    private void Start()
    {

        GameInput.Instance.OnChangePlayerStatePressed += GameInput_OnChangePlayerStatePressed;
        GameInput.Instance.OnPlayerInteractPressed += GameInput_OnPlayerInteractPressed;
        GameInput.Instance.OnPlayerInteractReleased += GameInput_OnPlayerInteractReleased;

        OnHealthUpdated?.Invoke(this, new OnHealthUpdatedArgs { maxHealth = maxHealth, updatedHealth = health });
        OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedArgs { playerState = currentPlayerState });

    }

    private void GameInput_OnPlayerInteractReleased(object sender, EventArgs e)
    {
        if (currentInteractablePlanetObject)
        {
            currentInteractablePlanetObject.InteractStopped(this);
        }
        
    }

    private void GameInput_OnPlayerInteractPressed(object sender, EventArgs e)
    {
        if (currentInteractablePlanetObject)
        {
            currentInteractablePlanetObject.Interact(this);
        }
        

        // TODO: Shoot out ray or something to detect shit

    }
    private void Update()
    {
        
        Collider2D collider = GetInteractableObjects();
        if (collider != null && collider.TryGetComponent(out PlanetObject planetObject))
        {
            if (currentInteractablePlanetObject != planetObject)
            {
                currentInteractablePlanetObject = planetObject;
                currentInteractablePlanetObject.ShowInteractable();
            }
        }
        else
        {
            if (currentInteractablePlanetObject != null)
            {
                currentInteractablePlanetObject.HideInteractable();
            }
            currentInteractablePlanetObject = null;
        }
    }

    private Collider2D GetInteractableObjects()
    {
        float circleRadius = GetComponent<CircleCollider2D>().radius * 1.5f;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, circleRadius, Vector2.right, 0, interactableObjectsLayer);
        if (hit)
        {
            //hit.collider.GetComponent<PlanetObject>().Interact(this);
            return hit.collider;
        }
        return null;
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

    public void UseFuel()
    {
        currentOilAmount -= Time.deltaTime * fuelBurnRate;
        if (currentOilAmount < 0 ) 
        {
            currentOilAmount = 0;
        }
    }

    public void SetState(PlayerStates playerState)
    {
        currentPlayerState = playerState;
        OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedArgs { playerState = currentPlayerState });
    }

    public void AddOil(float oilAmount)
    {
        currentOilAmount += oilAmount;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetOilAmount()
    {
        return currentOilAmount;
    }
    public float GetMaxOilAmount()
    {
        return maxOilAmount;
    }

    public bool hasFuel()
    {
        return currentOilAmount > 0;
    }

 
}
