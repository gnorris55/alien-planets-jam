using UnityEngine;

public class SpaceEnemySpawner : MonoBehaviour
{
    [SerializeField] private SpaceEnemy spaceEnemy;
    [SerializeField] private Planet targetPlanet;
    [SerializeField] private float spawnRate = 5.0f;
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private AnimationCurve spawnRateAnimationCurve;



    private bool canSpawnEnemy = true;
    private float spawnTimer = 0f;
    private int amountOfEnemies = 0;
    private float currentAngleToPlanet;
    private float distanceFromPlanet;
    private float startTime = 0;



    private void Awake()
    {
        startTime = Time.time;
        spawnRate = spawnRateAnimationCurve.Evaluate((startTime - Time.time)/10f);
    }

    private void Start()
    {
        // for making sure that enemies are not spawned at the same time
        distanceFromPlanet = Vector3.Distance(transform.position, targetPlanet.transform.position);

        //spawnTimer = Random.RandomRange(0, spawnRate);
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

        currentAngleToPlanet +=  Time.deltaTime;

        float x = targetPlanet.transform.position.x + Mathf.Cos(currentAngleToPlanet) * distanceFromPlanet;
        float y = targetPlanet.transform.position.y + Mathf.Sin(currentAngleToPlanet) * distanceFromPlanet;
        transform.position = new Vector3(x, y, 0);

        if (canSpawnEnemy && amountOfEnemies < maxEnemies)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnRate)
            {
                SpaceEnemy spaceEnemyInstance = Instantiate(spaceEnemy, transform.position, Quaternion.identity).GetComponent<SpaceEnemy>();
                spaceEnemyInstance.SetTargetPlanet(targetPlanet);
                spaceEnemyInstance.OnEnemyDestroyed += SpaceEnemyInstance_OnEnemyDestroyed;
                amountOfEnemies++;
                spawnRate = spawnRateAnimationCurve.Evaluate((startTime - Time.time)/10f);
                spawnTimer = 0f;
            }
        }
    }

    private void SpaceEnemyInstance_OnEnemyDestroyed(object sender, Enemy.OnEnemyDestroyedArgs e)
    {
        amountOfEnemies--;
    }
}
