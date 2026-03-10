using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Planet : MonoBehaviour
{

    [SerializeField] private float gravityScalar;
    [SerializeField] private string planetName;
    [SerializeField] private float planetRadius;
    [SerializeField] private float atmosphereRadius;
    
    [SerializeField] private ParticleSystem buildingParticleSystem;
    [SerializeField] PlanetAtmosphere planetAtmosphere;
    [SerializeField] PlanetVisuals planetVisuals;

    private List<PlanetObject> planetStructures = new List<PlanetObject>();


    public void Start()
    {
        GetComponent<CircleCollider2D>().radius = planetRadius;
        planetAtmosphere.SetAtmosphereRadius(atmosphereRadius);

    }

    // Gets the position on the planet so that objects obey laws of gravity for each planet, i.e. moving around it
    public Vector3 GetPlanetPosition(float horizontalMovement, Vector2 currentDir, Vector3 position, float objectRadius, float objectSpeed)
    {

        float distanceFromPlanet = Vector3.Distance(position, transform.position);

        float angle = Mathf.Atan2(currentDir.y, currentDir.x);

        // This is the standard planet radius
        float radiusEffect = (4 * Mathf.PI) / (2 * Mathf.PI * planetRadius);

        float rotationAmount = horizontalMovement * objectSpeed * radiusEffect* Time.deltaTime;

        angle += rotationAmount;

        float positionRadius = Mathf.Clamp(distanceFromPlanet, planetRadius, 100 + objectRadius);
        
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * positionRadius + transform.position;
    }

    public void PlaceObjectOnPlanet(PlanetObject planetObject, Vector3 objectPosition, Vector3 objectDirection)
    {
        Vector3 positionOnPlanet = (objectPosition - transform.position).normalized * planetRadius;
        GameObject planetStructure = Instantiate(planetObject.gameObject, objectPosition, Quaternion.identity);
        planetStructure.transform.up = objectDirection;
        Instantiate(buildingParticleSystem, objectPosition, Quaternion.identity);

        planetStructures.Add(planetStructure.GetComponent<PlanetObject>());
    }

    public bool CanPlaceObjectOnPlanet(float size, Vector3 position)
    {
        foreach (PlanetObject planetStructure in planetStructures)
        {
            if (Vector3.Distance(planetStructure.transform.position, position) < (size / 2) + (planetStructure.GetWidth() / 2))
                return false;
        }

        return true;

    }

    public List<OilStorage> GetOilStorageStructuresOnPlanet()
    {
        List<OilStorage> oilStorageStructures = new List<OilStorage>();
        foreach (PlanetObject planetStructure in planetStructures)
        {
            if (planetStructure is OilStorage)
            {
                oilStorageStructures.Add((OilStorage)planetStructure);
                
            }


        }

        return oilStorageStructures;
    } 

    public void PlanetHit(Vector3 location, float hitAngle)
    {
        planetVisuals.CreatePlanetHitEffect(location, hitAngle);
    }

    public float GetPlanetRadius()
    {
        return planetRadius;
    }

    public float GetGravityScalar()
    {
        return gravityScalar;
    }

    public string GetPlanetName()
    {
        return planetName;
    }

    public List<PlanetObject> GetPlanetObjects()
    {
        return planetStructures;
    }

   
}

