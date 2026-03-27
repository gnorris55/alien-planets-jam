using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BuildingMenuCell : MonoBehaviour
{

    [SerializeField] private Button button;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI planetObjectOilPrice;
    [SerializeField] private AudioSource buttonClickedAudioSource;

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
        planetObjectOilPrice.text = planetStructureSO.oilPrice.ToString();
    }

    private void ButtonActivated()
    {

        OnPlanetStructureSelected?.Invoke(this, new OnPlanetStructureSelectedArgs { planetStructureSO = planetStructureSO});
        GetComponent<AudioSource>().Play();
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
