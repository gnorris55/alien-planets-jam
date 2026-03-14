using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.TryGetComponent(out Planet planetHit))
        {
            Vector3 planetHitDirection = (transform.position - currentPlanet.transform.position).normalized;
            float planetHitAngle = Mathf.Atan2(planetHitDirection.y, planetHitDirection.x) * Mathf.Rad2Deg;
            planetHit.PlanetHit(transform.position, planetHitAngle);

            Destroy(gameObject);
        }
        else if (collision.gameObject.TryGetComponent(out PlanetObject planetObject))
        {
            planetObject.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        
    }
}

