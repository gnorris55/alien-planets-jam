using UnityEngine;

public class Planet : MonoBehaviour
{

    [SerializeField] private float gravityScalar;
    [SerializeField] private float atmosphereRadius;
    [SerializeField] private string planetName;

    public float GetGravityScalar()
    {
        return gravityScalar;
    }

    public string GetPlanetName()
    {
        return planetName;
    }
}

