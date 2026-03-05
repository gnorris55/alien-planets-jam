using UnityEngine;

public class PlanetAtmosphere : MonoBehaviour
{
    [SerializeField] Planet planet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Planet GetPlanet()
    {
        return planet;
    }
}
