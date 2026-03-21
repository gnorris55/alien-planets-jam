using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;
using static MineralDeposit;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Player : MonoBehaviour
{

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
    public event EventHandler<OnMineralAmountUpdatedArgs> OnMineralAmountUpdated;
    public class OnMineralAmountUpdatedArgs : EventArgs
    {
        public float updatedMineralAmount;
        public float maxMineralAmount;
        public MineralDeposit.MineralType mineralType;
    }


    public enum PlayerStates
    {
        combat,
        building
    }

    [SerializeField] private float maxHealth;
    [SerializeField] private float maxOilAmount;
    [SerializeField] private float maxBlueMineralAmount;
    [SerializeField] private float maxRedMineralAmount;
    [SerializeField] private float maxYellowMineralAmount;
    [SerializeField] private float fuelBurnRate;
    [SerializeField] private LayerMask playerPlanetObjectsLayer;
    [SerializeField] private LayerMask neutralPlanetObjectsLayer;
    [SerializeField] private bool infiniteFuel;


    private float health;
    private float currentOilAmount;
    private float currentBlueMineralAmount;
    private float currentRedMineralAmount;
    private float currentYellowMineralAmount;
    private float totalOil = 0;

    private PlanetObject currentInteractablePlanetObject;
    private PlayerStates currentPlayerState = PlayerStates.combat;
    private Planet currentPlanet;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        Instance = this;
        health = maxHealth;
    }

    private void Start()
    {

        playerMovement = GetComponent<PlayerMovement>();

        GameInput.Instance.OnChangePlayerStatePressed += GameInput_OnChangePlayerStatePressed;
        GameInput.Instance.OnPlayerInteractPressed += GameInput_OnPlayerInteractPressed;
        GameInput.Instance.OnPlayerInteractReleased += GameInput_OnPlayerInteractReleased;

        StatsManager.Instance.OnGameObjectStatsUpdated += StatsManager_OnGameObjectStatsUpdated;

        OnHealthUpdated?.Invoke(this, new OnHealthUpdatedArgs { maxHealth = maxHealth, updatedHealth = health });

        UpdateMineralAmount(currentBlueMineralAmount, maxBlueMineralAmount, MineralType.blue);
        UpdateMineralAmount(currentYellowMineralAmount, maxYellowMineralAmount, MineralType.yellow);
        UpdateMineralAmount(currentRedMineralAmount, maxRedMineralAmount, MineralType.red);

        OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedArgs { playerState = currentPlayerState });

        StatsManager.Instance.GetGameObjectStats(StatsManager.ObjectType.player);
        currentOilAmount = maxOilAmount;
    }

    private void StatsManager_OnGameObjectStatsUpdated(object sender, StatsManager.OnGameObjectStatsUpgradedArgs e)
    {
        if (e.objectType == StatsManager.ObjectType.player)
        {
            maxOilAmount = e.upgradeValues.storageUpgradeValues.Evaluate(e.currentLevel)*100f;
        }
    }

    private void GameInput_OnPlayerInteractReleased(object sender, EventArgs e)
    {
        if (currentInteractablePlanetObject)
        {
            currentInteractablePlanetObject.InteractStopped();
        }
        
    }

    private void GameInput_OnPlayerInteractPressed(object sender, EventArgs e)
    {
        if (currentInteractablePlanetObject)
        {
            currentInteractablePlanetObject.Interact(this);
        }
        
    }

    private void Update()
    {

        PlanetObject interactablePlanetObject = GetInteractablePlanetObjects();
        if (interactablePlanetObject != null)
        {
            if (currentInteractablePlanetObject != interactablePlanetObject)
            {
                currentInteractablePlanetObject = interactablePlanetObject;
                currentInteractablePlanetObject.ShowInteractable();
            }
        }
        else if (currentInteractablePlanetObject != null)
        { 
            {
                currentInteractablePlanetObject.HideInteractable();
                currentInteractablePlanetObject.InteractStopped();
                currentInteractablePlanetObject = null;
            }
        }
    }

    public void UpdateTotalOil()
    {
        totalOil = currentOilAmount;
        if (currentPlanet)
        {
            totalOil += currentPlanet.GetStoredOil();
        }
    }

    public float GetTotalOil()
    {
        UpdateTotalOil();
        return totalOil;
    }

    public bool CanAffordUpgrade(float requiredOilAmount, float requiredBlueMineral = 0, float requiredYellowMineral = 0, float requiredRedMineral = 0)
    {
        totalOil = GetTotalOil();

        return (totalOil >= requiredOilAmount) && (currentBlueMineralAmount >= requiredBlueMineral*10f) 
                                               && (currentYellowMineralAmount >= requiredYellowMineral*10f)
                                               && (currentRedMineralAmount >= requiredRedMineral*10f);
    }

    public bool BuyUpgrade(float requiredOilAmount, float requiredBlueMineral = 0, float requiredYellowMineral = 0, float requiredRedMineral = 0)
    {
        if (CanAffordUpgrade(requiredOilAmount, requiredBlueMineral, requiredYellowMineral,requiredRedMineral))
        {
            
            useOil(requiredOilAmount*100);
            currentBlueMineralAmount -= requiredBlueMineral*10f;
            currentYellowMineralAmount -= requiredYellowMineral*10f;
            currentRedMineralAmount -= requiredRedMineral * 10f;

            UpdateMineralAmount(currentBlueMineralAmount, maxBlueMineralAmount, MineralType.blue);
            UpdateMineralAmount(currentYellowMineralAmount, maxYellowMineralAmount, MineralType.yellow);
            UpdateMineralAmount(currentRedMineralAmount, maxRedMineralAmount, MineralType.red);

            return true;
        }
        return false;
    }

    public bool BuyPlanetObject(float oilAmount)
    {
        if (HasOilAmount(oilAmount))
        {
            useOil(oilAmount);
            return true;
        }

        return false;
    }

    public bool HasOilAmount(float oilAmount)
    {
        totalOil = GetTotalOil();
        return totalOil >= oilAmount;
    }

    public void useOil(float oilAmount)
    {
        totalOil = GetOilAmount();
        List<OilStorage> oilStorageDevices = currentPlanet.GetSpecificPlanetObject<OilStorage>();
        foreach (OilStorage oilStorageDevice in oilStorageDevices)
        {

            oilAmount = oilStorageDevice.removeOil(oilAmount);
        }

        if (oilAmount > 0 && !infiniteFuel)
        {
            currentOilAmount -= oilAmount;
        }

        PlanetOilAmountUI.Instance.DisplayTotalPlanetOil();

    }

    private PlanetObject GetInteractablePlanetObjects()
    {
        
        float circleRadius = GetComponent<CircleCollider2D>().radius * 1.5f;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, circleRadius, Vector2.right, 0, (playerPlanetObjectsLayer | neutralPlanetObjectsLayer));
        
        PlanetObject closestInteractablePlanetObject = null;
        float distanceFromClosestInteractablePlanetObject = float.MaxValue;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent(out PlanetObject planetObject) && planetObject.IsInteractable())
            {
                float playerDistanceFromPlanetObject = Vector3.Distance(planetObject.transform.position, transform.position);
                if (playerDistanceFromPlanetObject < distanceFromClosestInteractablePlanetObject)
                {
                    distanceFromClosestInteractablePlanetObject = playerDistanceFromPlanetObject;
                    closestInteractablePlanetObject = planetObject;
                }
            }

        }

        return closestInteractablePlanetObject;

    } 


    private void GameInput_OnChangePlayerStatePressed(object sender, EventArgs e)
    {

        if (currentPlanet != null)
        {
            int numPlayerStates = Enum.GetNames(typeof(PlayerStates)).Length;
            int nextState = ((int)currentPlayerState + 1) % numPlayerStates;

            currentPlayerState = (PlayerStates)nextState;
            OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedArgs { playerState = currentPlayerState });
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

    public void UsePlayerFuel()
    {
        if (!infiniteFuel)
        {
            currentOilAmount -= Time.deltaTime * fuelBurnRate;
            if (currentOilAmount < 0)
            {
                currentOilAmount = 0;
            }

            PlanetOilAmountUI.Instance.DisplayTotalPlanetOil();
        }
    }

    public void SetState(PlayerStates playerState)
    {
        currentPlayerState = playerState;
        OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedArgs { playerState = currentPlayerState });
    }

    public float AddMineral(float mineralAmount, MineralDeposit.MineralType mineralType)
    {

        float leftOverMineral = 0;
        if (mineralType == MineralDeposit.MineralType.blue)
        {
            leftOverMineral = HarvestSpecificMineral(mineralAmount, ref currentBlueMineralAmount, ref maxBlueMineralAmount, mineralType);

        }
        if (mineralType == MineralDeposit.MineralType.yellow)
        {
            leftOverMineral = HarvestSpecificMineral(mineralAmount, ref currentYellowMineralAmount, ref maxYellowMineralAmount, mineralType);
        }
        if (mineralType == MineralDeposit.MineralType.red)
        {
            leftOverMineral = HarvestSpecificMineral(mineralAmount, ref currentRedMineralAmount, ref maxRedMineralAmount, mineralType);
        }

        return leftOverMineral;
    }

    private void UpdateMineralAmount(float currentMineralAmount, float maxMineralAmount, MineralDeposit.MineralType mineralType)
    {
        OnMineralAmountUpdated?.Invoke(this, new OnMineralAmountUpdatedArgs
        {
            updatedMineralAmount = currentMineralAmount,
            maxMineralAmount = maxMineralAmount,
            mineralType = mineralType
        });
    }

    private float HarvestSpecificMineral(float mineralAmount, ref float currentMineralAmount, ref float maxMineralAmount, MineralDeposit.MineralType mineralType)
    {
            currentMineralAmount += mineralAmount;
            float leftOverMineral = Mathf.Clamp(currentMineralAmount - maxMineralAmount, 0, maxMineralAmount);
            if (leftOverMineral > 0)
            {
                currentMineralAmount = maxMineralAmount;
            }
        UpdateMineralAmount(currentMineralAmount, maxMineralAmount, mineralType);

        
        return leftOverMineral;
    }

    public float AddOil(float oilAmount)
    {

        currentOilAmount += oilAmount;

        float leftOverOil = Mathf.Clamp(currentOilAmount - maxOilAmount, 0, maxOilAmount);
        if (leftOverOil > 0)
        {
            currentOilAmount = maxOilAmount;
        }
        PlanetOilAmountUI.Instance.DisplayTotalPlanetOil();

        return leftOverOil;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            playerMovement.EnteredPlanet();
            PlanetAtmosphere planetAtmosphere = collision.gameObject.GetComponent<PlanetAtmosphere>();
            currentPlanet = planetAtmosphere.GetPlanet();
            PlanetOilAmountUI.Instance.DisplayTotalPlanetOil();
            GeneralPlayerMenuUI.Instance.ShowUI();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            playerMovement.ExitedPlanet();
            SetState(Player.PlayerStates.combat);
            currentPlanet = null;
            PlanetOilAmountUI.Instance.DisplayTotalPlanetOil();
            GeneralPlayerMenuUI.Instance.HideUI();
        }
    }

    public Planet GetCurrentPlanet()
    {
        return currentPlanet;
    }

    public void SetCurrentPlanet(Planet planet)
    {
        currentPlanet = planet;
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
