using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class Planet : MonoBehaviour
{

    public event EventHandler <OnToggleEnemySpawningArgs>OnToggleEnemySpawning;
    public class OnToggleEnemySpawningArgs : EventArgs
    {
        public bool canSpawnEnemy;
    }

    [SerializeField] private float gravityScalar;
    [SerializeField] private string planetName;
    [SerializeField] private float planetRadius;
    [SerializeField] private float atmosphereRadius;
    [SerializeField] private int maxAmountEnemies = 10;

    
    [SerializeField] private ParticleSystem buildingParticleSystem;
    [SerializeField] PlanetAtmosphere planetAtmosphere;
    [SerializeField] PlanetVisuals planetVisuals;


    private List<PlanetObject> planetStructures = new List<PlanetObject>();
    private List<Enemy> planetEnemies = new List<Enemy>();


    public void Start()
    {
        GetComponent<CircleCollider2D>().radius = planetRadius;
        planetAtmosphere.SetAtmosphereRadius(atmosphereRadius);

    }

    // Gets the position on the planet so that objects obey laws of gravity for each planet, i.e. moving around it
    public Vector3 GetPlanetPosition(float horizontalMovement, Vector3 position, float objectRadius, float objectSpeed, float allowableRange = 100, Vector3? currentDir = null)
    {
        Vector3 planetToObjectDirection = (position - transform.position).normalized;
        Vector3 objectDirection = currentDir ?? planetToObjectDirection;

        float distanceFromPlanet = Vector3.Distance(position, transform.position);

        float angle = Mathf.Atan2(objectDirection.y, objectDirection.x);

        // This is the standard planet radius
        float radiusEffect = (4 * Mathf.PI) / (2 * Mathf.PI * planetRadius);

        float rotationAmount = horizontalMovement * objectSpeed * radiusEffect* Time.deltaTime;

        angle += rotationAmount;

        float positionRadius = Mathf.Clamp(distanceFromPlanet, planetRadius + objectRadius, planetRadius + allowableRange + objectRadius);
        
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * positionRadius + transform.position;
    }

    public void AddObjectOnPlanet(PlanetObject planetObject, Vector3 objectPosition, Vector3 objectDirection)
    {
        Vector3 positionOnPlanet = (objectPosition - transform.position).normalized * planetRadius;
        PlanetObject planetObjectInstance = Instantiate(planetObject.gameObject, objectPosition, Quaternion.identity).GetComponent<PlanetObject>();

        planetObjectInstance.transform.up = objectDirection;
        Instantiate(buildingParticleSystem, objectPosition, Quaternion.identity);
        
        planetStructures.Add(planetObjectInstance);

        planetObjectInstance.SetHomePlanet(this);
        planetObjectInstance.OnPlanetObjectDestroyed += PlanetObject_OnPlanetObjectDestroyed;
    }

    public void AddEnemyOnPlanet(Enemy enemy)
    {
        planetEnemies.Add(enemy);
        enemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;

        if (planetEnemies.Count >= maxAmountEnemies)
        {
            OnToggleEnemySpawning?.Invoke(this, new OnToggleEnemySpawningArgs { canSpawnEnemy = false });
        }
    }

    private void Enemy_OnEnemyDestroyed(object sender, Enemy.OnEnemyDestroyedArgs e)
    {
        planetEnemies.Remove(e.enemy);

        if (planetEnemies.Count < maxAmountEnemies)
        { 
            OnToggleEnemySpawning?.Invoke(this, new OnToggleEnemySpawningArgs { canSpawnEnemy = true });
        }
    }

    private void PlanetObject_OnPlanetObjectDestroyed(object sender, PlanetObject.OnPlanetObjectDestroyedArgs e)
    {
        planetStructures.Remove(e.planetObject);
    }

    public bool CanPlaceObjectOnPlanet(float size, Vector3 position)
    {
        foreach (PlanetObject planetStructure in planetStructures)
        {
            if (Vector3.Distance(planetStructure.transform.position, position) < (size / 2) + (planetStructure.GetWidth() / 2))
                return false;
        }

        return true;

    }

    public List<OilStorage> GetOilStorageStructuresOnPlanet()
    {
        List<OilStorage> oilStorageStructures = new List<OilStorage>();
        foreach (PlanetObject planetStructure in planetStructures)
        {
            if (planetStructure is OilStorage)
            {
                oilStorageStructures.Add((OilStorage)planetStructure);
                
            }


        }

        return oilStorageStructures;
    } 

    public void PlanetHit(Vector3 location, float hitAngle)
    {
        planetVisuals.CreatePlanetHitEffect(location, hitAngle);
    }

    public float GetPlanetRadius()
    {
        return planetRadius;
    }

    public float GetGravityScalar()
    {
        return gravityScalar;
    }

    public string GetPlanetName()
    {
        return planetName;
    }

    public bool IsMaxAmountEnemies()
    {
        if (maxAmountEnemies > planetEnemies.Count)
        {
            return false;
        }

        return true;
    }
    public List<PlanetObject> GetPlanetObjects()
    {
        return planetStructures;
    }

   
}

