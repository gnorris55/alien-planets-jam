using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{


    public static StatsManager Instance { get; private set; }
    
    public enum ObjectType
    {
        player,
        oilRig,
        oilStorage,
        smallTurret
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


    private int playerLevel = 0;
    private int oilRigLevel = 0;
    private int oilStorageLevel = 0;
    private int smallTurretLevel = 0;


    private void Awake()
    {
        Instance = this;
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


    public void GetGameObjectStats(ObjectType objectType)
    {
        print(objectType);
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
