using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class RepairBuilding : PlanetObject
{
    [SerializeField] private float repairRadius = 2.0f;
    [SerializeField] private float repairAmount;
    [SerializeField] private LayerMask playerPlanetObjectsLayer;
    [SerializeField] private Transform repairRadiusVisualTransform;


    private List<PlanetObject> planetObjectsInRepairRadius = new List<PlanetObject>();

    private float checkRepairRadiusTime = 1.0f;
    private float checkRepairRadiusTimer = 0;
    private float repairTime = 0.25f;
    private float repairTimer = 0;

    private void Start()
    {
        base.Start();
        StatsManager.Instance.GetGameObjectStats(StatsManager.ObjectType.repairBuilding);
        repairRadiusVisualTransform.localScale *= repairRadius;
        StatsManager.Instance.OnGameObjectStatsUpdated += StatsManager_OnGameObjectStatsUpdated;
    }

    private void StatsManager_OnGameObjectStatsUpdated(object sender, StatsManager.OnGameObjectStatsUpgradedArgs e)
    {
        if (e.objectType == StatsManager.ObjectType.repairBuilding)
        {
            
            float updatedMaxHealthAmount = e.upgradeValues.healthUpgradeValues.Evaluate(e.currentLevel) * 100f;
            SetMaxHealth(updatedMaxHealthAmount);
            SetHealth(GetMaxHealth());
            statsSet = true;
        }
    }

    private void Update()
    {
        repairTimer -= Time.deltaTime;
        checkRepairRadiusTimer -= Time.deltaTime;

        if (repairTimer < 0 )
        {
            repairBuildingsInRepairRadius();
            repairTimer = repairTime;
        }
        if (checkRepairRadiusTimer < 0 )
        {
            GetBuildingsInRepairRadius();
            checkRepairRadiusTimer = checkRepairRadiusTime;
        }

    }

    private void repairBuildingsInRepairRadius()
    {
        // removes any objects that are destroyed
        planetObjectsInRepairRadius.RemoveAll(item => item == null);
        foreach (PlanetObject planetObject in planetObjectsInRepairRadius) 
        {
            planetObject.AddHealth(repairAmount);

        }
    }

    private void GetBuildingsInRepairRadius()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, repairRadius, Vector2.right, 0, playerPlanetObjectsLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent(out PlanetObject planetObject) && !planetObjectsInRepairRadius.Contains(planetObject))
            {
                planetObjectsInRepairRadius.Add(planetObject);
            }
        }
    }

    public override void InteractAlternate()
    {
        DestoryPlanetObject();
    }
    public override void ShowInteractable()
    {
        PlayerHints.Instance.DisplayHint("PRESS F TO DESTROY");
    }

}
