using System;
using UnityEngine;

public class OilStorage : PlanetObject, IOilStorageDevice
{

    public event EventHandler OnOilUpdatedInStorageDevice;

    [SerializeField] private float maxOilAmount = 100.0f;
    [SerializeField] private float oilHarvestSpeed = 25;
    [SerializeField] private ItemVisualMovement oilGlobVisual;

    private float currentOilAmount = 0f;
    private bool playerIsHarvestingOil = false;

    private float oilTransferedCount = 0;


    protected override void Start()
    {
        base.Start();
        StatsManager.Instance.GetGameObjectStats(StatsManager.ObjectType.oilStorage);
        SetHealth(GetMaxHealth());
    }


    protected override void StatsManager_OnGameObjectStatsUpdated(object sender, StatsManager.OnGameObjectStatsUpgradedArgs e)
    {
        if (e.objectType == StatsManager.ObjectType.oilStorage)
        {
            float updatedMaxOilAmount = e.upgradeValues.storageUpgradeValues[e.currentLevel];
            float updatedMaxHealthAmount = e.upgradeValues.healthUpgradeValues[e.currentLevel];
            maxOilAmount = updatedMaxOilAmount;
            SetMaxHealth(updatedMaxHealthAmount);

        }
    }

    public override void Interact(Player player)
    {
        playerIsHarvestingOil = true;
        if (currentOilAmount > 0)
        {
            isInteractable = true;
        }
    }

    public override void InteractStopped()
    {
        playerIsHarvestingOil = false;
    }

    public override void ShowInteractable()
    {
        PlayerHints.Instance.DisplayHint("HOLD E TO HARVEST OIL");
    }


    public void Update()
    {   
        if (playerIsHarvestingOil && currentOilAmount > 0)
        {
            TransferOilToPlayer();
        }
    }


    public float GetOilAmount()
    {
        return currentOilAmount;
    }

    public float GetMaxOilAmount()
    {
        return maxOilAmount;
    }

    public float AddOil(float oilAmount)
    {
        currentOilAmount += oilAmount;

        float leftOverOil = Mathf.Clamp(currentOilAmount - maxOilAmount, 0, maxOilAmount);
        if (leftOverOil > 0)
        {
            currentOilAmount = maxOilAmount;
        }

        OnOilUpdatedInStorageDevice?.Invoke(this, EventArgs.Empty);
        
        

        return leftOverOil;
    }

    private void TransferOilToPlayer()
    {

        float oilTransferAmount = Time.deltaTime * oilHarvestSpeed;
        

        Player player = Player.Instance;

        float leftOverOil = player.AddOil(oilTransferAmount);

        if (leftOverOil == 0)
        {
            DisplayOilTransferVisuals(oilTransferAmount, player.transform.position);

        }


        OnOilUpdatedInStorageDevice?.Invoke(this, EventArgs.Empty);
        currentOilAmount = currentOilAmount - oilTransferAmount + leftOverOil;


        if ((currentOilAmount / maxOilAmount) == 0.00)
        {
            isInteractable = false;
        }
        

        if ((currentOilAmount / maxOilAmount) < 0.01)
        {
            InteractStopped();
        }
    }

    private void DisplayOilTransferVisuals(float transferAmount, Vector3 transferTargetLocation)
    {
        oilTransferedCount += transferAmount;
        if (oilTransferedCount > 10)
        {
            ItemVisualMovement oilGlobInstance = Instantiate(oilGlobVisual, transform.position, Quaternion.identity);
            oilGlobInstance.SetUp(transform.position, transferTargetLocation);
            oilTransferedCount = 0;
        }

    }

    public override void TakeDamage(float damageAmount)
    {
        DamageStructure(damageAmount);
        if (IsDestroyed())
        {
            currentOilAmount = 0;
            OnOilUpdatedInStorageDevice?.Invoke(this, EventArgs.Empty);

            DestoryPlanetObject();
        }
    }
}
