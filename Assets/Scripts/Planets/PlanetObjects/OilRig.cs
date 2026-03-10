using System;
using UnityEngine;

public class OilRig : PlanetObject
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

    public override void InteractStopped(Player player)
    {
        playerIsHarvestingOil=false;
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
        }
        if (playerIsHarvestingOil && currentOilAmount > 0)
        {

            TransferOilToPlayer();
            
            //Player.Instance.AddOil()
        }
    }


    private void fillOil()
    {
        currentOilAmount += oilAccumulationSpeed * Time.deltaTime;

        if (currentOilAmount > maxOilAmount)
        {
            currentOilAmount = maxOilAmount;
        }
    }
    private void TransferOilToPlayer()
    {

        Player player = Player.Instance;
        
        float oilTransferAmount = Time.deltaTime * oilHarvestSpeed;
        if ((player.GetOilAmount() + oilTransferAmount) > player.GetMaxOilAmount())
        {
            float oilTankRoomLeft = player.GetMaxOilAmount() - player.GetOilAmount();
            player.AddOil(oilTankRoomLeft);
            currentOilAmount -= oilTankRoomLeft;
            playerIsHarvestingOil = false;
        }
        else
        {
            if (currentOilAmount < oilTransferAmount)
            {
                player.AddOil(currentOilAmount);
                currentOilAmount = 0;
                playerIsHarvestingOil = false;
            }
            else
            {
                player.AddOil(oilTransferAmount);    
                currentOilAmount -= oilTransferAmount;
            }
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
}
