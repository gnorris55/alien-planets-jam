using UnityEngine;

public class PlanetAtmosphere : MonoBehaviour
{
    [SerializeField] Planet planet;


    public void SetAtmosphereRadius(float atmosphereRadius)
    {
        GetComponent<CircleCollider2D>().radius = atmosphereRadius;
    }

    public Planet GetPlanet()
    {
        return planet;
    }
}
