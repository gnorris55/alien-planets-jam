using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyRadar : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    [SerializeField] private float radarRadius = 2;
    [SerializeField] private LayerMask playerPlanetObjectsLayer;

    private float scanTime = 1f;
    private float scanTimer;
    private List<Type> planetObjectPriorityList = new List<Type>()
    {
        typeof(OilStorage),
        typeof(OilRig),
        typeof(Turret)
    };

    private void Update()
    {
        if (!enemy.HasTarget())
        {
            scanTimer -= Time.deltaTime;

            if (scanTimer <= 0)
            {
                GetEnemiesInRadarRadius();
                scanTimer = scanTime;
            }
        }
    }

    private void GetEnemiesInRadarRadius()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radarRadius, Vector2.right, 0, playerPlanetObjectsLayer);
        
        foreach (RaycastHit2D hit in hits)
        {
           if (hit.collider.TryGetComponent(out PlanetObject planetObject))
            {
                enemy.SetTarget(planetObject);
                return;
            }
        }
        
    }

    private List<PlanetObjectChild> GetSpecificPlanetObjectHit<PlanetObjectChild>(RaycastHit2D[] hits)
    {
        
        List<PlanetObjectChild> planetObjectChildList = new List<PlanetObjectChild>();


        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent(out PlanetObjectChild planetObjectChild))
            {
                planetObjectChildList.Add(planetObjectChild);
            }


        }

        return planetObjectChildList;
    }
}
