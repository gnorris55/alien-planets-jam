using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GeneralPlayerMenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    static public GeneralPlayerMenuUI Instance { get; private set; }
    [SerializeField] private BuildingMenu buildingMenu;
    [SerializeField] private UpgradesMenuUI upgradesMenu;
    [SerializeField] private Button selectBuildingMenuButton;
    [SerializeField] private Button selectUpgradesMenuButton;
    [SerializeField] private Button ExitButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        selectBuildingMenuButton.onClick.AddListener(ShowBuildingMenu);
        selectUpgradesMenuButton.onClick.AddListener(ShowUpgradesMenu);
        ExitButton.onClick.AddListener(HidePlayerUI);

        HidePlayerUI();

    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        if (this)
        {
            gameObject.SetActive(false);
        }
    }

    private void ShowBuildingMenu()
    {
        
        buildingMenu.Show();
        upgradesMenu.Hide();
        ExitButton.gameObject.SetActive(true);
        Player.Instance.SetState(Player.PlayerStates.building);
    }

    private void ShowUpgradesMenu()
    {
        buildingMenu.Hide();
        upgradesMenu.Show();
        ExitButton.gameObject.SetActive(true);
        Player.Instance.SetState(Player.PlayerStates.combat);

    }

    private void HidePlayerUI()
    {
        buildingMenu.Hide();
        upgradesMenu.Hide();
        ExitButton.gameObject.SetActive(false);
        Player.Instance.SetState(Player.PlayerStates.combat);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameInput.Instance.ToggleShootInput(false);
        // Put your logic here (e.g., showing a tooltip or changing color)
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GameInput.Instance.ToggleShootInput(true);
    }
}
