using UnityEngine;
using UnityEngine.UI;

public class OilProgressBar : MonoBehaviour
{

    [SerializeField] private Image oilProgressFill;
    [SerializeField] private GameObject oilStorageDeviceGameObject;


    private IOilStorageDevice oilStorageDevice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!oilStorageDeviceGameObject.TryGetComponent(out oilStorageDevice))
        {
            Debug.LogError("GameObject does not contain interface for oil storage");
        }
        

        
    }

    private void Update()
    {
        oilProgressFill.fillAmount = oilStorageDevice.GetOilAmount() / oilStorageDevice.GetMaxOilAmount();
    }
}
