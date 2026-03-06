using UnityEngine;

public class Planet : MonoBehaviour
{

    [SerializeField] private float gravityScalar;
    [SerializeField] private string planetName;
    [SerializeField] private float planetRadius;
    [SerializeField] private float atmosphereRadius;

    [SerializeField] PlanetAtmosphere planetAtmosphere;

    public void Start()
    {
        GetComponent<CircleCollider2D>().radius = planetRadius;
        planetAtmosphere.SetAtmosphereRadius(atmosphereRadius);
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

