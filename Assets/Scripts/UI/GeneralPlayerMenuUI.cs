using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GeneralPlayerMenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BuildingMenu buildingMenu;
    [SerializeField] private UpgradesMenuUI upgradesMenu;
    [SerializeField] private Button selectBuildingMenuButton;
    [SerializeField] private Button selectUpgradesMenuButton;
    [SerializeField] private Button selectCombatButton;

    private void Start()
    {
        selectBuildingMenuButton.onClick.AddListener(ShowBuildingMenu);
        selectUpgradesMenuButton.onClick.AddListener(ShowUpgradesMenu);
        selectCombatButton.onClick.AddListener(HidePlayerUI);

        HidePlayerUI();

    }

    private void ShowBuildingMenu()
    {
        buildingMenu.Show();
        upgradesMenu.Hide();
    }

    private void ShowUpgradesMenu()
    {
        buildingMenu.Hide();
        upgradesMenu.Show();

    }

    private void HidePlayerUI()
    {
        buildingMenu.Hide();
        upgradesMenu.Hide();

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
