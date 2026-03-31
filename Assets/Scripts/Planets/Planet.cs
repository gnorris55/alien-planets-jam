using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;


public class Planet : MonoBehaviour
{
    public event EventHandler OnPlanetOilUpdated;
    public event EventHandler <OnToggleEnemySpawningArgs>OnToggleEnemySpawning;
    public class OnToggleEnemySpawningArgs : EventArgs
    {
        public bool canSpawnEnemy;
    }

    [SerializeField] private float gravityScalar;
    [SerializeField] private string planetName;
    [SerializeField] private float planetRadius;
    [SerializeField] private float atmosphereRadius;

    [SerializeField] private bool hasEnemies = false;
    [SerializeField] private PlanetStructureSO enemySpawnerStructureSO;
    [SerializeField] private int maxAmountEnemies = 10;
    
    [SerializeField] private ParticleSystem buildingParticleSystem;
    [SerializeField] private PlanetAtmosphere planetAtmosphere;
    [SerializeField] private PlanetVisuals planetVisuals;
    [SerializeField] private List<PlanetStructureSO> initialPlanetObjectSOList;

    [SerializeField] private PlanetStructureSO rocketShipSO; 

    [SerializeField] private bool isMainPlanet;

    private List<PlanetObject> planetStructures = new List<PlanetObject>();
    private List<Enemy> planetEnemies = new List<Enemy>();

    private void Awake()
    {
        GetComponent<CircleCollider2D>().radius = planetRadius;
        planetAtmosphere.SetAtmosphereRadius(atmosphereRadius);

        PlaceInitialPlanetStructures();
    }

    private void PlaceInitialPlanetStructures()
    {


        if (isMainPlanet)
        {
            Vector3 positionOnPlanet = transform.position + (new Vector3(0, 1, 0)) * planetRadius;
            Vector3 planetObjectPlanetPosition = GetPlanetPosition(0, positionOnPlanet, (rocketShipSO.height / 2.0f) - 0.02f, 0, 0);

            Vector3 planetObjectDirection = (positionOnPlanet - transform.position).normalized;

            AddObjectOnPlanet(rocketShipSO.structureGameObject, planetObjectPlanetPosition, planetObjectDirection);
        }

        foreach (PlanetStructureSO planetObjectSO in initialPlanetObjectSOList)
        {

            Vector3 positionOnPlanet = UnityEngine.Random.insideUnitCircle.normalized * planetRadius;

            Vector3 planetObjectPlanetPosition = GetPlanetPosition(0, positionOnPlanet, (planetObjectSO.height / 2.0f) - 0.02f, 0, 0);
            int count = 0;
            do
            {
                count++;
                if (count > 10)
                {
                    print("could not find a good position");
                    break;
                }

                Vector3 direction = UnityEngine.Random.insideUnitCircle.normalized;
                positionOnPlanet = transform.position + direction * planetRadius;

                planetObjectPlanetPosition = GetPlanetPosition(0, positionOnPlanet, (planetObjectSO.height / 2.0f) - 0.02f, 0, 0);

            } while (!CanPlaceObjectOnPlanet(planetObjectSO.width, planetObjectPlanetPosition));

            Vector3 planetObjectDirection = (positionOnPlanet - transform.position).normalized;
            AddObjectOnPlanet(planetObjectSO.structureGameObject, planetObjectPlanetPosition, planetObjectDirection);
        }
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

        if (planetObjectInstance.gameObject.TryGetComponent(out IOilStorageDevice oilStorageDevice))
        {
            oilStorageDevice.OnOilUpdatedInStorageDevice += OilStorageDevice_OnOilUpdatedInStorageDevice;
        }

    }


    private void OilStorageDevice_OnOilUpdatedInStorageDevice(object sender, EventArgs e)
    {
        Player.Instance.UpdateTotalOil();
        PlanetOilAmountUI.Instance.DisplayTotalPlanetOil();
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

        if (e.planetObject.gameObject.TryGetComponent(out IOilStorageDevice oilStorageDevice))
        {
            oilStorageDevice.OnOilUpdatedInStorageDevice -= OilStorageDevice_OnOilUpdatedInStorageDevice;
        }

        planetStructures.Remove(e.planetObject);
    }

    public bool CanPlaceObjectOnPlanet(float size, Vector3 position)
    {
        float allowablePlacementDistance = 0.05f;
        foreach (PlanetObject planetStructure in planetStructures)
        {
            float combinedStructurePlacementWidth = (size / 2f) + (planetStructure.GetWidth() / 2f) + allowablePlacementDistance;
            if (Vector3.Distance(planetStructure.transform.position, position) < combinedStructurePlacementWidth)
                return false;
        }

        return true;

    }

    public List<PlanetObjectChild> GetSpecificPlanetObject<PlanetObjectChild>() where PlanetObjectChild : PlanetObject
    {
        List<PlanetObjectChild> planetObjectChildList = new List<PlanetObjectChild>();
        foreach (PlanetObject planetStructure in planetStructures)
        {
            if (planetStructure is PlanetObjectChild)
            {
                planetObjectChildList.Add((PlanetObjectChild)planetStructure);
                
            }


        }

        return planetObjectChildList;
    } 

    public void PlanetHit(Vector3 location, float hitAngle)
    {
        planetVisuals.CreatePlanetHitEffect(location, hitAngle);
    }

    public float GetStoredOil()
    {

        float totalOilAmount = 0;
        foreach (PlanetObject planetObject in planetStructures)
        {
            if (planetObject.gameObject.TryGetComponent(out OilStorage oilStorage))
            {
                totalOilAmount += oilStorage.GetOilAmount();
            }
        }

        return totalOilAmount;
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

