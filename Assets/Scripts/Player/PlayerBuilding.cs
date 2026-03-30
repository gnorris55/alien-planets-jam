using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class PlayerBuilding : MonoBehaviour
{


    public static PlayerBuilding Instance { get; private set; }
    [SerializeField] private PlanetStructureSO currentPlanetStructureSO;
    [SerializeField] private float placementDistance = -1.1f;

    [SerializeField] private AudioSource placeObjectAudioSource;
    [SerializeField] private AudioSource cantPlaceObjectAudioSource;

    private bool isBuilding = false;
    private bool canPlaceObject = true;
    private Transform planetStructurePlacementVisualTransform;
    private Player player;
        
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = Player.Instance;
        GameInput.Instance.OnShootInputPressed += GameInput_OnShootInputPressed;
        Player.Instance.OnPlayerStateChanged += Player_OnPlayerStateChanged;
       
    }

    private void GameInput_OnShootInputPressed(object sender, System.EventArgs e)
    {
        if (isBuilding && canPlaceObject)
        {

            if (canPlaceObject)
            {

                BuildCurrentStructure(); 
            }
        }

    }

    private void Player_OnPlayerStateChanged(object sender, Player.OnPlayerStateChangedArgs e)
    {
        if (e.playerState == Player.PlayerStates.building)
        {
            isBuilding = true;
            if (planetStructurePlacementVisualTransform == null)
            {
                planetStructurePlacementVisualTransform = Instantiate(currentPlanetStructureSO.structureOutline, transform.position, Quaternion.identity).transform;
                SetColorOverlayForObjectPlacement(new Vector4(1, 0, 0, 1));

            }
        }
        else
        {
            isBuilding = false;
            if (planetStructurePlacementVisualTransform != null)
            {
                Destroy(planetStructurePlacementVisualTransform.gameObject);
            }
        }
    }

    private void BuildCurrentStructure()
    {
        if (Player.Instance.BuyPlanetObject(currentPlanetStructureSO.oilPrice))
        {
            Planet currentPlanet = player.GetCurrentPlanet();
            GetObjectPlacement(out Vector3 placementLocation, out Vector3 placementDirection);
            currentPlanet.AddObjectOnPlanet(currentPlanetStructureSO.structureGameObject, placementLocation, placementDirection);

            PlanetOilAmountUI.Instance.DisplayTotalPlanetOil();

            placeObjectAudioSource.Play();
            CameraManager.Instance.ShakeCamera(1f, 0.1f);
        }
        else
        {
            //cantPlaceObjectAudioSource.Play();
        }
    }


    private void GetObjectPlacement(out Vector3 placementLocation,  out Vector3 placementDirection)
    {
        Planet currentPlanet = player.GetCurrentPlanet();
        float planetStructureSize = currentPlanetStructureSO.height / 2.0f - 0.02f;
        placementLocation = currentPlanet.GetPlanetPosition(placementDistance, player.transform.position, planetStructureSize , 0.75f, 0, Player.Instance.transform.up);
        placementDirection = (placementLocation - currentPlanet.transform.position).normalized;
    }

    private void SetColorOverlayForObjectPlacement(Vector4 colorOverlay)
    {
        Material overlayMaterial = planetStructurePlacementVisualTransform.gameObject.GetComponent<SpriteRenderer>().material;
        overlayMaterial.SetColor("_OverlayColor", colorOverlay);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            GetObjectPlacement(out Vector3 placementLocation, out Vector3 placementDirection);
            planetStructurePlacementVisualTransform.position = placementLocation;
            planetStructurePlacementVisualTransform.up = placementDirection;

            Planet currentPlanet = player.GetCurrentPlanet();

            canPlaceObject = currentPlanet.CanPlaceObjectOnPlanet(currentPlanetStructureSO.width, planetStructurePlacementVisualTransform.position);
            
            if (canPlaceObject && Player.Instance.HasOilAmount(currentPlanetStructureSO.oilPrice))
            {
                SetColorOverlayForObjectPlacement(new Vector4(0, 1, 0, 1));
            }
            else
            {
                SetColorOverlayForObjectPlacement(new Vector4(1, 0, 0, 1));
            }
        }
    }


    public void SetPlanetStructureSO(PlanetStructureSO planetStructureSO)
    {
        currentPlanetStructureSO = planetStructureSO;
        if (planetStructurePlacementVisualTransform != null)
            Destroy(planetStructurePlacementVisualTransform.gameObject);

        planetStructurePlacementVisualTransform = Instantiate(currentPlanetStructureSO.structureOutline, transform.position, Quaternion.identity).transform;
    }
}
