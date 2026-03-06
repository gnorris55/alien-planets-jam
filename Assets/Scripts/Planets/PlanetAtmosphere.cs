using UnityEngine;

public class PlanetAtmosphere : MonoBehaviour
{
    [SerializeField] Planet planet;
    [SerializeField] Transform atmosphereVisual;


    public void SetAtmosphereRadius(float atmosphereRadius)
    {
        GetComponent<CircleCollider2D>().radius = atmosphereRadius;
        atmosphereVisual.localScale *= atmosphereRadius;
    }

    public Planet GetPlanet()
    {
        return planet;
    }
}
