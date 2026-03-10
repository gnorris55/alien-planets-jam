using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BuildingMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private PlanetStructuresSO planetStructuresSO;
    [SerializeField] private BuildingMenuCell buildMenuCell;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Player.Instance.OnPlayerStateChanged += Player_OnPlayerStateChanged;
        foreach(PlanetStructureSO planetStructureSO in planetStructuresSO.planetStructures) 
        {
            BuildingMenuCell currentCell = Instantiate(buildMenuCell, this.transform).GetComponent<BuildingMenuCell>();
            currentCell.SetUp(planetStructureSO);
            currentCell.OnPlanetStructureSelected += CurrentCell_OnPlanetStructureSelected;

        }
    }

    private void Player_OnPlayerStateChanged(object sender, Player.OnPlayerStateChangedArgs e)
    {
        if (e.playerState == Player.PlayerStates.building)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void CurrentCell_OnPlanetStructureSelected(object sender, BuildingMenuCell.OnPlanetStructureSelectedArgs e)
    {
        PlayerBuilding.Instance.SetPlanetStructureSO(e.planetStructureSO);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameInput.Instance.ToggleShootInput(false);
        // Put your logic here (e.g., showing a tooltip or changing color)
        Debug.Log("Mouse entered the building menu!");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse left the building menu!");
        GameInput.Instance.ToggleShootInput(true);
    }

}
