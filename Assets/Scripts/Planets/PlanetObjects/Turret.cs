using Unity.VisualScripting;
using UnityEngine;

public class Turret : PlanetObject
{
    [SerializeField] private Transform turretGun;
    [SerializeField] private Transform turretProjectileSpawnLocation;

    [SerializeField] private Bullet projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;

    [SerializeField] private float turretShootTime = 2f;

    private float turretShootTimer = 0;

    private Enemy targetedEnemy;


    
    public void SetEnemyTarget(Enemy enemy)
    {
        targetedEnemy = enemy;
    }

    private void Update()
    {
        if (targetedEnemy) 
        {
            Vector3 directionToEnemy = (targetedEnemy.transform.position - turretGun.position).normalized;
            turretGun.right = directionToEnemy;

            turretShootTimer -= Time.deltaTime;
            if (turretShootTimer <= 0)
            {
                SpawnProjectile(directionToEnemy);
                turretShootTimer = turretShootTime;
            }
        }
    }

    private void SpawnProjectile(Vector3 shootDirection)
    {
        Bullet bulletInstance = Instantiate(projectile.gameObject, turretProjectileSpawnLocation.position, Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.Setup(shootDirection, projectileDamage, projectileSpeed);
    }

    public Enemy GetEnemyTarget() 
    { 
        return targetedEnemy;
    }


}


