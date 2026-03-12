using UnityEngine;

public class FlyingEnemy : Enemy
{

    [SerializeField] private float flightHeight;
    [SerializeField] private float speedRange;

    private Planet homePlanet;
    private float movementDirection;


    protected override void Start()
    {
        base.Start();
        speed = Random.value * speedRange + speed;
        movementDirection = (Random.value > 0.5f) ? -1 : 1;

    }

    private void Update()
    {
        if (homePlanet != null)
        {
            float distanceFromPlanet = Vector3.Distance(homePlanet.transform.position, transform.position);

            if (distanceFromPlanet >= flightHeight)
            {
                transform.position = homePlanet.GetPlanetPosition(movementDirection, transform.position, 0.1f, speed, 100);
            }
            else
            {
                Vector3 planetUpDirection = (transform.position - homePlanet.transform.position);
                transform.position += planetUpDirection * Time.deltaTime*speed;
                transform.position = homePlanet.GetPlanetPosition(0, transform.position, 0.1f, speed, 100);
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            PlanetAtmosphere planetAtmosphere = collision.gameObject.GetComponent<PlanetAtmosphere>();
            homePlanet = planetAtmosphere.GetPlanet();
        }
    }

    public void SetFlightHeight(float flightHeight)
    {
        this.flightHeight = flightHeight;
    }
}
