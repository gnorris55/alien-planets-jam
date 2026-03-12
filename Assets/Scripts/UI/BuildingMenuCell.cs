using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BuildingMenuCell : MonoBehaviour
{

    [SerializeField] private Button button;
    [SerializeField] private Image itemImage;

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
        itemImage.sprite = planetStructureSO.sprite;
    }

    private void ButtonActivated()
    {

        OnPlanetStructureSelected?.Invoke(this, new OnPlanetStructureSelectedArgs { planetStructureSO = planetStructureSO});
    }

    public void OnEnterButton()
    {
        transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.25f).SetEase(Ease.OutBack); ;
    }

    public void OnExitButton()
    {
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f).SetEase(Ease.InQuad);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
