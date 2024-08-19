using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyModels;
    public Material[] enemyMaterials;
    //public GameObject[] enemySpawnPoints;
    public int minEnemies = 1;
    public int maxEnemies = 5;
    public float spawnObjRadius = 1000f;
    public int numberOfSpawnPoints = 4;
    public float spawnRadius = 50f;
    public float spawnInterval = 2f;
    public AudioClip explosionSound;
    public float countdownTime = 40f;
    public Text countdownText;
    public float minXZ = -5000;
    public float maxXZ = 5000;
    public static EnemySpawner instance;

    private int wave;
    //private List<string> gameModes = new List<string>() {"TimingMode","AnnMode" };
    private List<string> gameModes = new List<string>() { "TimingMode" };
    private float playerY;
    private bool isSpawning = true;
    private Transform playerTransform;
    private int remainingEnemy = 0;

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
            // 在XZ平面上生成随机半径和角度
            float randomRadius = Random.Range(0, spawnRadius);
            float randomAngle = Random.Range(0, Mathf.PI * 2);

            // 将极坐标转换为直角坐标
            float x = Mathf.Cos(randomAngle) * randomRadius;
            float z = Mathf.Sin(randomAngle) * randomRadius;

            // 计算相对于玩家位置的生成点
            randomPosition = new Vector3(playerTransform.position.x + x, playerTransform.position.y, playerTransform.position.z + z);

        } while (randomPosition.x < minXZ || randomPosition.x > maxXZ || randomPosition.z < minXZ || randomPosition.z > maxXZ);

        // 返回有效范围内的随机位置
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
                AnnModeSpawnEnemies();
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

        if (GameObject.FindGameObjectsWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectsWithTag("Player")[0].transform;
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
        navMeshAgent.stoppingDistance = 20;

        // EnemyAI
        EnemyAI enemyScript = enemy.AddComponent<EnemyAI>();
        enemyScript.chaseDistance = 1000;

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
        Debug.Log(countdownTime);
        ScenceOver();
    }
}
