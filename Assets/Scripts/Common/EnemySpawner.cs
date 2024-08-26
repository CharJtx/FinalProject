using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyModels;
    public Material[] enemyMaterials;
    public GameObject[] bulletPrefab;
    public BattleTimer battleTimer;
    public EnemyLeft enemyLeft;
    //public GameObject[] enemySpawnPoints;
    public int minEnemies = 1;
    public int maxEnemies = 5;
    public float spawnObjRadius = 1000f;
    public int numberOfSpawnPoints = 4;
    public float spawnRadius = 50f;
    public float spawnInterval = 2f;
    public AudioClip explosionSound;
    public float countdownTime = 120f ;
    public Text countdownText;
    public float minXZ = -5000;
    public float maxXZ = 5000;
    public static EnemySpawner instance;

    // Enemy AI setting
    public float fireCutDown = 5f;
    public float attackDistance = 100;
    public float chaseDistance = 10000;
    public float speed = 15f;

    public int wave = 10;
    //private List<string> gameModes = new List<string>() { "TimingMode", "AnnMode" };
    private List<string> gameModes = new List<string>() { "AnnMode" };
    private float playerY;
    private bool isSpawning = true;
    private Transform playerTransform;
    public int remainingEnemy = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerTransform = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        playerY = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y;
        ChooseMode();
        
        
    }

    Vector3 GetRandomPositionAroundPlayer()
    {
        Vector3 randomPosition;
        do
        {
            // Generate random radii and angles in the XZ plane
            float randomRadius = Random.Range(0, spawnRadius);
            float randomAngle = Random.Range(0, Mathf.PI * 2);

            // Convert polar coordinates to cartesian coordinates
            float x = Mathf.Cos(randomAngle) * randomRadius;
            float z = Mathf.Sin(randomAngle) * randomRadius;

            // Calculate the generation point relative to the player's position
            randomPosition = new Vector3(playerTransform.position.x + x, playerTransform.position.y, playerTransform.position.z + z);

        } while (randomPosition.x < minXZ || randomPosition.x > maxXZ || randomPosition.z < minXZ || randomPosition.z > maxXZ);

        // Return to a random location within the effective range
        return randomPosition;
    }

    void ChooseMode()
    {
        int modeNumber = Random.Range(0,gameModes.Count);
        switch (gameModes[modeNumber])
        {
            case "TimingMode":
                StartCoroutine(TimingModeSpawnEnemies());
                StartCoroutine(Countdown());
                break;
            case "AnnMode":
                StartCoroutine(AnnModeSpawnEnemies());
                break;
        }
    }

    IEnumerator TimingModeSpawnEnemies()
    {
        while (isSpawning)
        {
            int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator AnnModeSpawnEnemies()
    {
        if (enemyLeft != null)
        {
            enemyLeft.showWindow = true;
        }

        wave = Random.Range(5, 20);
        while (wave > 0)
        {
            int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
            }
            wave -= 1;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator Countdown()
    {

        if (battleTimer !=null)
        {
            battleTimer.showWindow = true;
        }

        while (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            //Debug.Log(timeRemaining);
            //countdownText.text = timeRemaining.ToString() + "S";
            yield return null;
        }
        //countdownText.text = "Time's UP";
        countdownTime = 0;
        isSpawning = false;
    }

    void SpawnEnemy()
    {
        if (enemyModels.Length == 0 || enemyMaterials.Length == 0) return;

        //GameObject spawnObj;
        //if (enemySpawnPoints.Length != 0)
        //{
        //    spawnObj = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
        //}
        //else
        //{
        //    spawnObj = gameObject;
        //}

        if (PlayerController.instance != null)
        {
            playerTransform = PlayerController.instance.transform;
        }
        

        // Choose a model and material randomly
        GameObject enemyModel = enemyModels[Random.Range(0, enemyModels.Length)];
        Material enemyMaterial = enemyMaterials[Random.Range(0, enemyMaterials.Length)];

        // Generate an enemy
        GameObject enemy = new GameObject("Enemy");

        // Instantiate the model as a child of the enemy
        GameObject enemyInstance = Instantiate(enemyModel, enemy.transform);

        // Ensure the instance has the necessary components
        MeshRenderer meshRenderer = enemyInstance.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = enemyInstance.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = enemyMaterial;

        // rigibody
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = enemy.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // colider
        BoxCollider collider = enemyInstance.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = enemyInstance.AddComponent<BoxCollider>();
        }

        // NavMesh
        NavMeshAgent navMeshAgent = enemy.AddComponent<NavMeshAgent>();
        navMeshAgent.height = 0;
        navMeshAgent.angularSpeed = 10;
        navMeshAgent.stoppingDistance = attackDistance/2;
        navMeshAgent.speed = speed;

        // EnemyAI
        EnemyAI enemyScript = enemy.AddComponent<EnemyAI>();
        enemyScript.bulletPrefab = bulletPrefab[Random.Range(0,bulletPrefab.Length)];
        enemyScript.chaseDistance = chaseDistance;
        enemyScript.attackDistance = attackDistance;
        enemyScript.attackCooldown = fireCutDown;

        // healty
        SmallEnemyHealth health = enemyInstance.AddComponent<SmallEnemyHealth>();
        health.explosionClip = explosionSound;

        // Set enemy spawn position randomly
        //Vector3 spawnPosition = spawnObj.transform.position + Random.insideUnitSphere * spawnRadius;
        Vector3 spawnPosition = GetRandomPositionAroundPlayer();
        spawnPosition.y = playerY;
        enemy.transform.position = spawnPosition;
        remainingEnemy += 1;

        // set enemy layer
        enemy.layer = 8;
        enemyInstance.layer = 8;
    }

    void ScenceOver()
    {
        if (countdownTime == 0 || (remainingEnemy == 0 && wave == 0))
        {
            MapGenerator.Instance.ShowPanel();
        }
    }

    public void EnemyDeath(int i)
    {
        remainingEnemy -= i;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(countdownTime);
        ScenceOver();
    }
}
