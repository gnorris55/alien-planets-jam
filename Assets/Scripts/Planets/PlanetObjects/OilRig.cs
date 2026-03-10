using System;
using UnityEngine;

public class OilRig : PlanetObject, IOilStorageDevice
{
    [SerializeField] private float maxOilAmount = 100.0f;
    [SerializeField] private float oilAccumulationSpeed = 0.05f;
    [SerializeField] private float oilHarvestSpeed = 100f;

    public event EventHandler OnOilRigFilled;

    private float currentOilAmount = 0;
    private bool playerIsHarvestingOil = false;

    public override void Interact(Player player)
    {

        playerIsHarvestingOil = true;
    }

    public override void InteractStopped()
    {
        playerIsHarvestingOil = false;
    }

    public override void ShowInteractable()
    {
        PlayerHints.Instance.DisplayHint("HOLD E TO HARVEST OIL");
    }

    private void Update()
    {
        if (currentOilAmount < maxOilAmount)
        {
            fillOil();
            if (!isInteractable && (currentOilAmount / maxOilAmount) > 0.1)
            {
                isInteractable = true;
            }
        }
        if (playerIsHarvestingOil && currentOilAmount > 0)
        {

            TransferOilToPlayer();
            
            //Player.Instance.AddOil()
        }
    }


    private void fillOil()
    {
        float oilProduced = oilAccumulationSpeed * Time.deltaTime;

        AddOil(oilProduced);
    }
    private void TransferOilToPlayer()
    {

        Player player = Player.Instance;
        
        float oilTransferAmount = Time.deltaTime * oilHarvestSpeed;
        
        float leftOverOil = player.AddOil(oilTransferAmount);

        currentOilAmount = currentOilAmount - oilTransferAmount + leftOverOil;

        if ((currentOilAmount / maxOilAmount) < 0.05)
        {
            isInteractable = false;
        }

        if ((currentOilAmount / maxOilAmount) < 0.01)
        {
            InteractStopped();
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

        return leftOverOil;

    }
}
