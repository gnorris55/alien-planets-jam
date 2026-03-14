using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OilProgressBar : MonoBehaviour
{

    [SerializeField] private Image oilProgressFill;
    [SerializeField] private GameObject oilStorageDeviceGameObject;
    [SerializeField] private GameObject oilAmountTextGameObject;


    private IOilStorageDevice oilStorageDevice;
    private TextMeshPro oilAmountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        oilAmountText = oilAmountTextGameObject.GetComponent<TextMeshPro>();
        if (!oilStorageDeviceGameObject.TryGetComponent(out oilStorageDevice))
        {
            Debug.LogError("GameObject does not contain interface for oil storage");
        }
    }

    private void Update()
    {
        oilProgressFill.fillAmount = oilStorageDevice.GetOilAmount() / oilStorageDevice.GetMaxOilAmount();
        oilAmountText.text = (int)oilStorageDevice.GetOilAmount() + "/" + (int)oilStorageDevice.GetMaxOilAmount();
    }
}
