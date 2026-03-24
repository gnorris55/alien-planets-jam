using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{

    private const int PLAYER_MAX_LEVEL = 5;
    private const int OIL_RIG_MAX_LEVEL = 5;
    private const int OIL_STORAGE_MAX_LEVEL = 5;
    private const int SMALL_TURRET_MAX_LEVEL = 5;
    private const int ROCKETSHIP_MAX_LEVEL = 1;

    public static StatsManager Instance { get; private set; }
    
    public enum ObjectType
    {
        player,
        oilRig,
        oilStorage,
        smallTurret,
        rocketShip
    }


    public event EventHandler<OnGameObjectStatsUpgradedArgs> OnGameObjectStatsUpdated;
    public class OnGameObjectStatsUpgradedArgs
    {
        public ObjectType objectType;
        public UpgradeValuesSO upgradeValues;
        public int currentLevel;
    }

    [SerializeField] private UpgradeValuesSO playerUpgradeValuesSO;
    [SerializeField] private UpgradeValuesSO oilRigUpgradeValuesSO;
    [SerializeField] private UpgradeValuesSO oilStorageUpgradeValuesSO;
    [SerializeField] private UpgradeValuesSO smallTurretUpgradeValuesSO;
    [SerializeField] private UpgradeValuesSO rocketShipUpgradeValuesSO;


    private int playerLevel = 0;
    private int oilRigLevel = 0;
    private int oilStorageLevel = 0;
    private int smallTurretLevel = 0;
    private int rocketShipLevel = 0;


    private void Awake()
    {
        Instance = this;
    }

    public void UpgradeStats(ObjectType type)
    {
        switch(type)
        {
            case ObjectType.player:
                UpgradeSpecificStat(ref playerLevel, UpdatePlayerStats);
                break;
            case ObjectType.oilRig: 
                UpgradeSpecificStat(ref oilRigLevel, UpdateOilRigStats);
                break;
            case ObjectType.oilStorage:
                UpgradeSpecificStat(ref oilStorageLevel, UpdateOilStorageStats);
                break;
            case ObjectType.smallTurret:
                UpgradeSpecificStat(ref smallTurretLevel, UpdateSmallTurretStats);
                break;
            case ObjectType.rocketShip:
                UpgradeSpecificStat(ref rocketShipLevel, UpdateRocketShipStats);
                break;
        }
    }

    public bool IsMaxLevel(ObjectType type)
    {
        switch(type)
        {
            case ObjectType.player: 
                return playerLevel >= PLAYER_MAX_LEVEL;
            case ObjectType.oilRig:
                return oilRigLevel >= OIL_RIG_MAX_LEVEL;
            case ObjectType.oilStorage:
                return oilStorageLevel >= OIL_STORAGE_MAX_LEVEL;
            case ObjectType.smallTurret:
                return smallTurretLevel >= SMALL_TURRET_MAX_LEVEL;
            case ObjectType.rocketShip:
                return rocketShipLevel >= ROCKETSHIP_MAX_LEVEL;
        }

        return false;
    }

    public int GetCurrentLevel(ObjectType type)
    {
        switch(type)
        {
            case ObjectType.player:
                return playerLevel;
            case ObjectType.oilRig:
                return oilRigLevel;
            case ObjectType.oilStorage:
                return oilStorageLevel;
            case ObjectType.smallTurret:
                return smallTurretLevel;
            case ObjectType.rocketShip:
                return rocketShipLevel;

        }

        return -1;
    }

    private void UpgradeSpecificStat(ref int objectLevel, Action updateFunction)
    {
        objectLevel++;
        updateFunction?.Invoke();
    }
    private void UpdatePlayerStats()
    {
        OnGameObjectStatsUpdated?.Invoke(this, new OnGameObjectStatsUpgradedArgs 
        { objectType = ObjectType.player, 
            upgradeValues = playerUpgradeValuesSO, 
            currentLevel = playerLevel
        });
    }
    private void UpdateOilRigStats()
    {
        OnGameObjectStatsUpdated?.Invoke(this, new OnGameObjectStatsUpgradedArgs 
        { objectType = ObjectType.oilRig, 
            upgradeValues = oilRigUpgradeValuesSO, 
            currentLevel = oilRigLevel
        });
    }

    private void UpdateOilStorageStats()
    {
        OnGameObjectStatsUpdated?.Invoke(this, new OnGameObjectStatsUpgradedArgs 
        { objectType = ObjectType.oilStorage, 
            upgradeValues = oilStorageUpgradeValuesSO, 
            currentLevel = oilStorageLevel
        });
    }

    private void UpdateSmallTurretStats()
    {
        OnGameObjectStatsUpdated?.Invoke(this, new OnGameObjectStatsUpgradedArgs 
        { objectType = ObjectType.smallTurret, 
            upgradeValues = smallTurretUpgradeValuesSO, 
            currentLevel = smallTurretLevel
        });
    }
    private void UpdateRocketShipStats()
    {
        OnGameObjectStatsUpdated?.Invoke(this, new OnGameObjectStatsUpgradedArgs 
        { objectType = ObjectType.rocketShip, 
            upgradeValues = rocketShipUpgradeValuesSO, 
            currentLevel = rocketShipLevel
        });
    }

    

    public void GetGameObjectStats(ObjectType objectType)
    {
        switch(objectType)
        {
            case ObjectType.player:
                UpdatePlayerStats();
                break;
            case ObjectType.oilRig:
                UpdateOilRigStats(); 
                break;
            case ObjectType.oilStorage:
                UpdateOilStorageStats();
                break;
            case ObjectType.smallTurret:
                UpdateSmallTurretStats(); 
                break;
        }
    }





}
