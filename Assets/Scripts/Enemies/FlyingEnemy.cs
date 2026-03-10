using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{

    [SerializeField] private float flightHeight;
    private Planet currentPlanet;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            PlanetAtmosphere planetAtmosphere = collision.gameObject.GetComponent<PlanetAtmosphere>();
            currentPlanet = planetAtmosphere.GetPlanet();
        }
    }


    private void Update()
    {
        if (currentPlanet != null)
        {
           
        }
    }

    public void SetFlightHeight(float flightHeight)
    {
        this.flightHeight = flightHeight;
    }
}
