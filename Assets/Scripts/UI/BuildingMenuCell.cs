using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuCell : MonoBehaviour
{

    [SerializeField] private Button button;

    public event EventHandler<OnPlanetStructureSelectedArgs> OnPlanetStructureSelected;
    public class OnPlanetStructureSelectedArgs : EventArgs
    {
        public PlanetStructureSO planetStructureSO;
    }

    private PlanetStructureSO planetStructureSO;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(ButtonActivated); 
    }

    public void SetUp(PlanetStructureSO planetStructureSO)
    {
        this.planetStructureSO = planetStructureSO;
        //button.image.sprite = planetStructureSO.sprite;
    }

    private void ButtonActivated()
    {
        OnPlanetStructureSelected?.Invoke(this, new OnPlanetStructureSelectedArgs { planetStructureSO = planetStructureSO});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
