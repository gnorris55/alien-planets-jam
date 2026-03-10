using UnityEngine;

public class OilStorage : PlanetObject, IOilStorageDevice
{

    [SerializeField] private float maxOilAmount = 100.0f;
    private float currentOilAmount = 0f;
    public override void ShowInteractable()
    {
        PlayerHints.Instance.DisplayHint("HOLD E TO HARVEST OIL");
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
