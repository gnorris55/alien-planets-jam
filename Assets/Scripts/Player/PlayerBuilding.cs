using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class PlayerBuilding : MonoBehaviour
{

    [SerializeField] PlanetStructureSO currentPlanetStructureSO;
    [SerializeField] GameObject playerParent;



    private bool isBuilding = false;
    private bool canPlaceObject = true;
    private float placementDistance = -1.1f;
    private Transform planetStructurePlacementVisualTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameInput.Instance.OnShootInputPressed += GameInput_OnShootInputPressed;
        Player.Instance.OnPlayerStateChanged += Player_OnPlayerStateChanged;
        
    }

    private void GameInput_OnShootInputPressed(object sender, System.EventArgs e)
    {
        if (isBuilding && canPlaceObject)
        {
            buildCurrentStructure(); 
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

    private void buildCurrentStructure()
    {

        Planet currentPlanet = playerParent.GetComponent<PlayerMovement>().GetCurrentPlanet();
        GetObjectPlacement(out Vector3 placementLocation, out Vector3 placementDirection);
        currentPlanet.PlaceObjectOnPlanet(currentPlanetStructureSO.structureGameObject, placementLocation, placementDirection);
    }


    private void GetObjectPlacement(out Vector3 placementLocation,  out Vector3 placementDirection)
    {
        PlayerMovement playerMovement = playerParent.GetComponent<PlayerMovement>();
        Planet currentPlanet = playerMovement.GetCurrentPlanet();
        placementLocation = playerMovement.GetNewPlanetPosition(placementDistance, playerMovement.transform.up, currentPlanet.GetPlanetRadius());
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

            Planet currentPlanet = playerParent.GetComponent<PlayerMovement>().GetCurrentPlanet();

            canPlaceObject = currentPlanet.CanPlaceObjectOnPlanet(currentPlanetStructureSO.size, planetStructurePlacementVisualTransform.position);
            
            if (canPlaceObject)
            {
                SetColorOverlayForObjectPlacement(new Vector4(0, 1, 0, 1));
            }
            else
            {
                SetColorOverlayForObjectPlacement(new Vector4(1, 0, 0, 1));
            }
        }
    }
}
