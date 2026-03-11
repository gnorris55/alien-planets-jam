using UnityEngine;

public interface IOilStorageDevice
{
    public float AddOil(float oilAmount);
    public float GetOilAmount();

    public float GetMaxOilAmount();

    


}
