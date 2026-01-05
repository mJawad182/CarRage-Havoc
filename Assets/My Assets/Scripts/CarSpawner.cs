using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
 
    [Header("References")]
    public GameObject enemyPrefab;        // Enemy prefab to spawn
    private Transform player;            // Player transform (car), found by tag

    [Header("Spawn Settings")]
    public int poolSize = 20;            // Number of enemies in the pool
    public float spawnRadius = 15f;      // Radius around player to spawn enemies
    public float spawnInterval = 2f;     // Time between spawn attempts
    public int maxActiveEnemies = 40;    // Max enemies active at once

    private List<GameObject> enemyPool;
    private float spawnTimer = 0f;
    private float totalTime = 0f;


    void Start()
    {
        // Find player by tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("EnemySpawner: Player with tag 'Player' not found!");

        // Initialize pool
        enemyPool = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    void Update()
    {
        if (player == null)
            return;
        totalTime += Time.deltaTime;
        if (totalTime > 360f)
            return; // Stop spawning after 240 seconds
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnEnemy();
        }
    }

    void TrySpawnEnemy()
    {
        // Count active enemies
        int activeCount = 0;
        foreach (var enemy in enemyPool)
        {
            if (enemy.activeInHierarchy)
                activeCount++;
        }

        if (activeCount >= maxActiveEnemies)
            return; // Too many active enemies, skip spawn

        // Find an inactive enemy in the pool
        GameObject enemyToSpawn = null;
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemyToSpawn = enemy;
                break;
            }
        }

        if (enemyToSpawn == null)
            return; // No available enemy in pool

        // Calculate random spawn position around player within spawnRadius
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

        // Adjust Y if needed (e.g., ground level)
        spawnPos.y = 0f;

        // Activate and position enemy
        enemyToSpawn.transform.position = spawnPos;
        enemyToSpawn.transform.rotation = Quaternion.identity;
        enemyToSpawn.GetComponent<EnemyHealth>().currentHealth = enemyToSpawn.GetComponent<EnemyHealth>().maxHealth;
        enemyToSpawn.SetActive(true);

        // Optional: Reset enemy state if your enemy script supports it
        //var enemyScript = enemyToSpawn.GetComponent<EnemyMovement>();
        //if (enemyScript != null)
        //{
        //    enemyScript.ResetEnemy();
        //}
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}

