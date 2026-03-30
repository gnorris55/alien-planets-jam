using DG.Tweening;
using UnityEngine;

public class FlyingEnemy : Enemy
{

    [SerializeField] private EnemyBullet bullet;
    [SerializeField] private float flightHeight;
    [SerializeField] private float speedRange;

    private float movementDirection;


    protected override void Start()
    {
        base.Start();
        speed = Random.value * speedRange + speed;
        movementDirection = (Random.value > 0.5f) ? -1 : 1;

    }

    private void Update()
    {
        EnemyActionsBasedOnState();
    }

    private void EnemyActionsBasedOnState()
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
        if (homePlanet != null)
        {
            float distanceFromPlanet = Vector3.Distance(homePlanet.transform.position, transform.position) - homePlanet.GetPlanetRadius();
            Vector3 directionFromPlanet = (transform.position - homePlanet.transform.position);

            if (distanceFromPlanet >= flightHeight)
            {
                transform.position = homePlanet.GetPlanetPosition(direction, transform.position, 0.1f, speed, 100);
                if (planetObjectTarget != null)
                {
                    currentState = EnemyState.targetFound;
                }
            }
            else
            {
                transform.position += directionFromPlanet * Time.deltaTime*speed;
                transform.position = homePlanet.GetPlanetPosition(0, transform.position, 0.1f, speed, 100);
            }


            transform.up = Vector3.Slerp(transform.up,  directionFromPlanet, Time.deltaTime * 5f);
        }

    }

    private void MoveTowardTarget()
    {
        if (homePlanet != null)
        {
            Vector3 directionToPlanetObject = (planetObjectTarget.transform.position - transform.position).normalized;
            Vector3 directionFromPlanet = (transform.position - homePlanet.transform.position);

            transform.position = homePlanet.GetPlanetPosition(movementDirection, transform.position, 0.1f, speed, 100);

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
    public override void SetTarget(PlanetObject planetObject)
    {
        base.SetTarget(planetObject);
        movementDirection = GetShortestDirection(transform, planetObject.transform, homePlanet.transform);
        if (movementDirection > 0)
        {
            enemySpriteRenderer.flipX = true;
            bulletSpawnTransform.localPosition = new Vector3(-1*bulletSpawnTransform.localPosition.x, bulletSpawnTransform.localPosition.y, bulletSpawnTransform.localPosition.z);
        }


    }
    private void ShootProjectile()
    {
        Vector3 directionToPlanetObject = (planetObjectTarget.transform.position - bulletSpawnTransform.position).normalized;
        EnemyBullet bulletInstance = Instantiate(bullet, bulletSpawnTransform.position, Quaternion.identity);
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

    public void SetFlightHeight(float flightHeight)
    {
        this.flightHeight = flightHeight;
    }
}
