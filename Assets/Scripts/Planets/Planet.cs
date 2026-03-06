using UnityEngine;

public class Planet : MonoBehaviour
{

    [SerializeField] private Transform planetVisual;
    [SerializeField] private float gravityScalar;
    [SerializeField] private string planetName;
    [SerializeField] private float planetRadius;
    [SerializeField] private float atmosphereRadius;

    [SerializeField] PlanetAtmosphere planetAtmosphere;

    public void Start()
    {
        GetComponent<CircleCollider2D>().radius = planetRadius;
        planetAtmosphere.SetAtmosphereRadius(atmosphereRadius);

        planetVisual.localScale *= planetRadius;
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
}

