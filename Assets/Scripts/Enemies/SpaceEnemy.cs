using Unity.Multiplayer.PlayMode;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceEnemy : Enemy
{


    [SerializeField] private float flightHeight;
    [SerializeField] private EnemyBullet bullet;

    private float movementDirection = -1;


    private Planet targetPlanet;


    public void SetTargetPlanet(Planet targetPlanet)
    {
        this.targetPlanet = targetPlanet;
    }

    private void Update()
    {
        if (homePlanet)
        {
            // move around Planet, same as flying enemy
            PlanetMovement();
        }
        else if (targetPlanet)
        {
            MoveTowardTargetPlanet();
        }
    }


    private void MoveTowardTargetPlanet()
    {

        Vector3 directionToTargetPlanet = (targetPlanet.transform.position - transform.position).normalized;

        transform.position += directionToTargetPlanet * Time.deltaTime * speed*5f;
    }
    private void PlanetMovement()
    {
        switch (currentState)
        {
            case EnemyState.noTarget:
                HoverInDirection(movementDirection);
                break;
            case EnemyState.targetFound:
                MoveTowardTarget();
                break;
            case EnemyState.targetInAttackRange:
                ShootAtPlanetObject();
                break;
        }
    }

    private void HoverInDirection(float direction)
    {
        if (targetPlanet != null)
        {
            float distanceFromPlanet = Vector3.Distance(targetPlanet.transform.position, transform.position);
            Vector3 directionFromPlanet = (transform.position - targetPlanet.transform.position);
            if (distanceFromPlanet <= flightHeight)
            {
                transform.position = targetPlanet.GetPlanetPosition(direction, transform.position, 0.1f, speed, 100);
                if (planetObjectTarget != null)
                {
                    currentState = EnemyState.targetFound;
                }
            }
            else
            {
                transform.position -= directionFromPlanet * Time.deltaTime * speed;
                transform.position = targetPlanet.GetPlanetPosition(0, transform.position, 0.1f, speed, 100);
            }


            transform.up = Vector3.Slerp(transform.up, directionFromPlanet, Time.deltaTime * 5f);
        }

    }
    private void MoveTowardTarget()
    {
        if (targetPlanet != null)
        {
            Vector3 directionToPlanetObject = (planetObjectTarget.transform.position - transform.position).normalized;
            Vector3 directionFromPlanet = (transform.position - targetPlanet.transform.position);
            //float horizontalMovement = directionToPlanetObject.x > 0 ? -1f : 1f;
            float horizontalMovement = GetShortestDirection(transform, planetObjectTarget.transform, targetPlanet.transform);

            //transform.position += directionToPlanetObject * Time.deltaTime * speed;



            transform.position = targetPlanet.GetPlanetPosition(horizontalMovement, transform.position, 0.1f, speed, 100);

            if (Vector3.Distance(planetObjectTarget.transform.position, transform.position) < attackRange)
            {
                currentState = EnemyState.targetInAttackRange;
            }
            transform.up = Vector3.Slerp(transform.up, directionFromPlanet, Time.deltaTime * 5f);
        }
    }


    public int GetShortestDirection(Transform enemy, Transform target, Transform planet)
    {
        // 1. Get vectors from planet center to the objects
        Vector3 toEnemy = (enemy.position - planet.position).normalized;
        Vector3 toTarget = (target.position - planet.position).normalized;

        // 2. The 2D Cross Product (Det) formula: (x1 * y2) - (y1 * x2)
        float crossProduct = (toEnemy.x * toTarget.y) - (toEnemy.y * toTarget.x);

        // If crossProduct > 0, target is Counter-Clockwise
        // If crossProduct < 0, target is Clockwise
        return crossProduct > 0 ? 1 : -1;
    }
    private void ShootAtPlanetObject()
    {
        attackFrequencyTimer -= Time.deltaTime;
        if (attackFrequencyTimer <= 0)
        {
            ShootProjectile();
            attackFrequencyTimer = attackFrequencyTime;
        }
    }

    private void ShootProjectile()
    {
        Vector3 directionToPlanetObject = (planetObjectTarget.transform.position - transform.position).normalized;
        EnemyBullet bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
        bulletInstance.Setup(directionToPlanetObject, damage, 3);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            PlanetAtmosphere planetAtmosphere = collision.gameObject.GetComponent<PlanetAtmosphere>();
            homePlanet = planetAtmosphere.GetPlanet();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlanetAtmosphere")
        {
            homePlanet = null;
        }
    }
}
