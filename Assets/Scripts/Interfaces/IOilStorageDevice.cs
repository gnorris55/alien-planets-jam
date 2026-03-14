using UnityEngine;
using System;

public interface IOilStorageDevice
{

    public event EventHandler OnOilUpdatedInStorageDevice;
    public float AddOil(float oilAmount);
    public float GetOilAmount();

    public float GetMaxOilAmount();

    


}
