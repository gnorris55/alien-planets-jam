using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OilRig : PlanetObject, IOilStorageDevice
{
    [SerializeField] private float maxOilAmount = 100.0f;
    [SerializeField] private float oilAccumulationSpeed = 0.05f;
    [SerializeField] private float oilHarvestSpeed = 100f;
    [SerializeField] private ItemVisualMovement oilGlobVisual;

    public event EventHandler OnOilRigFilled;

    private float currentOilAmount = 0;
    private bool playerIsHarvestingOil = false;
    private float oilTransferedCount = 0;

    public override void Interact(Player player)
    {
        playerIsHarvestingOil = true;
    }

    public override void InteractStopped()
    {
        playerIsHarvestingOil = false;
        oilTransferedCount = 0;
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
        }
    }


    private void fillOil()
    {
        float oilProduced = oilAccumulationSpeed * Time.deltaTime;

        AddOil(oilProduced);
    }
    private void TransferOilToPlayer()
    {

        
        float oilTransferAmount = Time.deltaTime * oilHarvestSpeed;


        Player player = Player.Instance;

        float leftOverOil = player.AddOil(oilTransferAmount);

        currentOilAmount = currentOilAmount - oilTransferAmount + leftOverOil;

        // if there is left over oil, this means that the player has maxed out their oil storage
        // now we look to oil storage structures and see if they are full
        if  (leftOverOil > 0)
        {
            List<OilStorage> oilStorageStructures = player.GetCurrentPlanet().GetOilStorageStructuresOnPlanet();
            foreach(OilStorage oilStorageStructure in oilStorageStructures)
            {
                if (oilStorageStructure.GetOilAmount() < oilStorageStructure.GetMaxOilAmount())
                {
                    oilTransferAmount = leftOverOil;
                    DisplayOilTransferVisuals(oilTransferAmount, oilStorageStructure.transform.position);

                    leftOverOil = oilStorageStructure.AddOil(oilTransferAmount);
                    currentOilAmount = currentOilAmount - oilTransferAmount + leftOverOil;
                }
            }
        }
        else
        {
            DisplayOilTransferVisuals(oilTransferAmount, player.transform.position);
        }

        if ((currentOilAmount / maxOilAmount) < 0.05)
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
