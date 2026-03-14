using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using DG.Tweening;

public class BuildingMenu : MonoBehaviour{

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


    
    public void Show()
    {
        gameObject.SetActive(true);
        //transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f).SetEase(Ease.OutBack); ;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        //transform.DOScale(new Vector3(0.1f, 1f, 1f), 0.25f).SetEase(Ease.InQuad);
    }

    private void Player_OnPlayerStateChanged(object sender, Player.OnPlayerStateChangedArgs e)
    {
        if (e.playerState == Player.PlayerStates.building)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void CurrentCell_OnPlanetStructureSelected(object sender, BuildingMenuCell.OnPlanetStructureSelectedArgs e)
    {
        PlayerBuilding.Instance.SetPlanetStructureSO(e.planetStructureSO);
    }


}
