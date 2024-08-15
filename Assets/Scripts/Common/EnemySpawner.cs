using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyModels;
    public Material[] enemyMaterials;
    public int minEnemies = 1;
    public int maxEnemies = 5;
    public float spawnRadius = 10f;
    public float spawnInterval = 2f;
    public AudioClip explosionSound;

    private float playerY;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
        playerY = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y;
    }

    IEnumerator SpawnEnemies()
    {
        //while (true)
        {
            int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyModels.Length == 0 || enemyMaterials.Length == 0) return;

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
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = playerY;
        enemy.transform.position = spawnPosition;

        // set enemy layer
        enemy.layer = 8;
        enemyInstance.layer = 8;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
