using UnityEngine;

public class TurretBullet : Bullet
{

    private Enemy targetedEnemy;
    public void SetTarget(Enemy targetedEnemy)
    {
        this.targetedEnemy = targetedEnemy;
    }

    protected override void Update()
    {

        if (this.targetedEnemy != null)
        {


            Vector3 enemyDirection = (targetedEnemy.transform.position - transform.position).normalized;
            Vector3 currentVelocity = enemyDirection * speed;

            // if the bullet is in a planets atmosphere, its gravity will effect its movement
            if (currentPlanet != null)
            {
                Vector3 planetDirection = (currentPlanet.transform.position - transform.position).normalized;
                currentVelocity += planetDirection * currentPlanet.GetGravityScalar();
            }


            rb.linearVelocity = currentVelocity;
        }   else
        {
            base.Update();
        }
    }
}
