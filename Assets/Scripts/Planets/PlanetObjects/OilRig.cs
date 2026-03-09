using System;
using UnityEngine;

public class OilRig : PlanetObject
{
    [SerializeField] private float maxOilAmount = 100.0f;
    [SerializeField] private float oilAccumulationSpeed = 0.05f;

    public event EventHandler OnOilRigFilled;

    private float currentOilAmount = 0;


    public override void Interact(Player player)
    {
        
        if ((player.GetOilAmount() + currentOilAmount) > player.GetMaxOilAmount())
        {
            float oilTankRoomLeft = player.GetMaxOilAmount() - player.GetOilAmount();
            player.AddOil(oilTankRoomLeft);
            currentOilAmount -= oilTankRoomLeft;
        }
        else
        {
            player.AddOil(currentOilAmount);    
            currentOilAmount = 0;
        }
        
    }

    private void Update()
    {
        if (currentOilAmount < maxOilAmount)
        {
            fillOil();
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

    public float GetOilAmount()
    {
        return currentOilAmount;
    }

    public float GetMaxOilAmount()
    {
        return maxOilAmount;
    }
}
