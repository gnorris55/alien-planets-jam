using TMPro;
using UnityEngine;

public class PlanetOilAmountUI : MonoBehaviour
{

    public static PlanetOilAmountUI Instance;

    [SerializeField] private TextMeshProUGUI totalOilAmountText;
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
            float totalOilAmount = player.GetTotalOil();
            totalOilAmountText.text = currentPlanet.GetPlanetName() +"'s total stored oil: " + (int)totalOilAmount;
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
