using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlanetOilAmountUI : MonoBehaviour
{

    public static PlanetOilAmountUI Instance;

    [SerializeField] private TextMeshProUGUI totalOilAmountText;
    [SerializeField] private Image oilAmountFill;
    Player player;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = Player.Instance.GetComponent<Player>();
    }

    public void DisplayTotalPlanetOil()
    {
        Planet currentPlanet = player.GetCurrentPlanet();
        if (currentPlanet != null )
        {
            Show();

            // get oil storage devices
            List<OilStorage> oilStorageStructures = currentPlanet.GetSpecificPlanetObject<OilStorage>();
            float planetOilCapacity = 0f;

            foreach (OilStorage oilStorage in oilStorageStructures )
            {
                planetOilCapacity += oilStorage.GetMaxOilAmount();
            }


            float totalOilAmountOnPlanet = player.GetTotalOil();




            float playerMaxOil = player.GetMaxOilAmount();
            totalOilAmountText.text = currentPlanet.GetPlanetName() +"'s total stored oil: " + (int)totalOilAmountOnPlanet + " / " + (int)(playerMaxOil + planetOilCapacity);
            // TODO: Get max amount of oil

            oilAmountFill.fillAmount = totalOilAmountOnPlanet / (playerMaxOil + planetOilCapacity);
        }
        else
        {
            Hide();
        }

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (this)
        {
            gameObject.SetActive(false);
        }
    }

}
