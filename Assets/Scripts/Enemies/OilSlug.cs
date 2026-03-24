using UnityEngine;

public class OilSlug : Enemy
{

    [SerializeField] private float speedRange = 1.15f;
    private float movementDirection = -1f;
    protected override void Start()
    {
        base.Start();
        movementDirection = (Random.value > 0.5f) ? -1 : 1;
    }

    public override void SetUp(float speed, Vector3 direction)
    {
        base.SetUp(speed, direction);
    }

    private void Update()
    {
        if (homePlanet != null)
        {
            Vector3 directionFromPlanet = (transform.position - homePlanet.transform.position);
            transform.position = homePlanet.GetPlanetPosition(movementDirection, transform.position, 0.07f, speed, 0f);
            transform.up = Vector3.Slerp(transform.up,  directionFromPlanet, Time.deltaTime * 5f);
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

}
