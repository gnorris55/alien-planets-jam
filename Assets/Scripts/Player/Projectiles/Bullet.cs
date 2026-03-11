using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ParticleSystem groundExplosionParticleSystem;

    private float damageAmount;
    private Rigidbody2D rb;
    private Vector3 shootDirection;
    private Planet currentPlanet;
    

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

    }
    public void Setup(Vector3 shootDir, float damageAmount, float speed, float despawnTime = 2f)
    {
        this.speed = speed;
        this.damageAmount = damageAmount;

        transform.right = shootDir;
        shootDirection = shootDir;

        Destroy(gameObject, despawnTime);
    }

    private void Update()
    {

        Vector3 currentVelocity = shootDirection * speed;

        // if the bullet is in a planets atmosphere, its gravity will effect its movement
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

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            currentPlanet = null;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.TryGetComponent(out Planet planetHit))
        {

            Vector3 planetHitDirection = (transform.position - currentPlanet.transform.position).normalized;
            float planetHitAngle = Mathf.Atan2(planetHitDirection.y, planetHitDirection.x) * Mathf.Rad2Deg;
            planetHit.PlanetHit(transform.position, planetHitAngle);

            Destroy(gameObject);
        }
        else if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(20);
            Destroy(gameObject);
        }
        
    }
}
