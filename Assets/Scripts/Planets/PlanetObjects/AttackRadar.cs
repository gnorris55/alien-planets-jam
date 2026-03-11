using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class AttackRadar : MonoBehaviour
{

    [SerializeField] private Turret turret;
    [SerializeField] private GameObject attackRadiusVisual;
    

    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask detectingLayer;

    private List<Enemy> enemiesInRadarRadius = new List<Enemy>();
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<CircleCollider2D>().radius *= attackRadius;
        attackRadiusVisual.transform.localScale *= attackRadius;
    }
    private void Enemy_OnEnemyDestroyed(object sender, Enemy.OnEnemyDestroyedArgs e)
    {
        RemoveEnemyFromRadar(e.enemy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((detectingLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            AddEnemyToRadar(enemy);

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((detectingLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.OnEnemyDestroyed -= Enemy_OnEnemyDestroyed;

            RemoveEnemyFromRadar(enemy);
        }
    }

    private void AddEnemyToRadar(Enemy enemy)
    {
        enemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
        enemiesInRadarRadius.Add(enemy);
        
        Enemy currentTargetedEnemy = turret.GetEnemyTarget();

        if (currentTargetedEnemy == null)
        {
            turret.SetEnemyTarget(enemy);
        }
    }

    private void RemoveEnemyFromRadar(Enemy enemy)
    {
        enemiesInRadarRadius.Remove(enemy);
        Enemy currentTargetedEnemy = turret.GetEnemyTarget();

        if (currentTargetedEnemy == enemy)
        {
            turret.SetEnemyTarget(GetClosestEnemy());
        }
    }

    private Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        float closestEnemyDistance = float.MaxValue;
        foreach (Enemy enemy in enemiesInRadarRadius)
        {
            float distanceFromEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceFromEnemy < closestEnemyDistance)
            {
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
