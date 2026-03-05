using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed = 2f;

    private Rigidbody2D rb;
    private Vector3 shootDirection;
    private Planet currentPlanet;


    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Setup(Vector3 shootDir)
    {
        transform.right = shootDir;
        shootDirection = shootDir;
    }

    private void Update()
    {

        Vector3 currentVelocity = shootDirection * speed;


        if (currentPlanet != null)
        {
            Vector3 planetDirection = (currentPlanet.transform.position - transform.position).normalized;
            currentVelocity += planetDirection * currentPlanet.GetGravityScalar(); 
        }

        rb.linearVelocity = currentVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            PlanetAtmosphere planetAtmosphere = collision.gameObject.GetComponent<PlanetAtmosphere>();
            currentPlanet = planetAtmosphere.GetPlanet();
            Debug.Log("entering: " + currentPlanet.GetPlanetName());

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            currentPlanet = null;
        }
    }
}
