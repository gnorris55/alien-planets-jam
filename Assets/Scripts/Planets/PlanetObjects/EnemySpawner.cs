using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawner : PlanetObject
{
    [SerializeField] private GameObject enemyType;
    [SerializeField] private float spawnRate = 5.0f;

    private bool canSpawnEnemy = true;
    private float spawnTimer = 0f;

    private void Start()
    {
        // for making sure that enemies are not spawned at the same time
        spawnTimer = Random.RandomRange(0, spawnRate);
    }

    public override void SetHomePlanet(Planet planet)
    {
        homePlanet = planet;
        homePlanet.OnToggleEnemySpawning += HomePlanet_OnToggleEnemySpawning;
        canSpawnEnemy = !homePlanet.IsMaxAmountEnemies();
    }

    private void HomePlanet_OnToggleEnemySpawning(object sender, Planet.OnToggleEnemySpawningArgs e)
    {
        if (!e.canSpawnEnemy)
        {
            spawnTimer = 0f;
        }

        canSpawnEnemy = e.canSpawnEnemy;

    }

    private void Update()
    {
        if (canSpawnEnemy)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnRate)
            {
                Enemy enemyInstance = Instantiate(enemyType, transform.position, Quaternion.identity).GetComponent<Enemy>(); 
                homePlanet.AddEnemyOnPlanet(enemyInstance);
                spawnTimer = 0f;
            }
        }
    }


}
