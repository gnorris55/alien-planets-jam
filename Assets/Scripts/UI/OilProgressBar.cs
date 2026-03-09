using UnityEngine;
using UnityEngine.UI;

public class OilProgressBar : MonoBehaviour
{

    [SerializeField] private Image oilProgressFill;
    [SerializeField] private OilRig oilRig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void Update()
    {
        oilProgressFill.fillAmount = oilRig.GetOilAmount() / oilRig.GetMaxOilAmount();
    }
}
