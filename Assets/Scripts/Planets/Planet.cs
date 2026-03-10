using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Planet : MonoBehaviour
{

    [SerializeField] private Transform planetVisual;
    [SerializeField] private float gravityScalar;
    [SerializeField] private string planetName;
    [SerializeField] private float planetRadius;
    [SerializeField] private float atmosphereRadius;
    [SerializeField] private Transform miniMapVisual;
    [SerializeField] private ParticleSystem buildingParticleSystem;

    [SerializeField] PlanetAtmosphere planetAtmosphere;

    private List<PlanetObject> planetStructures = new List<PlanetObject>();


    public void Start()
    {
        GetComponent<CircleCollider2D>().radius = planetRadius;
        planetAtmosphere.SetAtmosphereRadius(atmosphereRadius);

        planetVisual.localScale *= planetRadius;
        miniMapVisual.localScale *= planetRadius;
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

