using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FuelBar : MonoBehaviour
{
    [SerializeField] private Image FuelFill;
    [SerializeField] private TextMeshProUGUI fuelAmountText;

    private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.Instance;
    }


    private void Update()
    {
        FuelFill.fillAmount = player.GetOilAmount() / player.GetMaxOilAmount();
        fuelAmountText.text = "Fuel: " + (int)player.GetOilAmount() + "/" + (int)player.GetMaxOilAmount();
    }

}

