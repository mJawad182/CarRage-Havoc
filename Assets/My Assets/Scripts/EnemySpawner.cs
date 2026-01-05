using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public List<GameObject> enemyPrefabs;  // List of 3 enemy prefabs
    private Transform player;

    [Header("Max Active Enemies Per Interval")]
    public List<int> maxEnemiesPerInterval = new List<int> { 20, 30, 40, 50, 60, 70, 80, 90 };

    [Header("Spawn Settings")]
    public int poolSizePerEnemy = 150;       // Pool size per enemy type
    public int baseMaxActiveEnemies = 20;  // Starting max enemies
    public int maxMaxActiveEnemies = 150;  // Max cap
    public float spawnRadius = 15f;
    public float spawnInterval = 2f;

    private int maxActiveEnemies;  // Will be updated dynamically

    private List<List<GameObject>> enemyPools; // Separate pools for each enemy type
    private float spawnTimer = 0f;
    private float totalTime = 0f;

    private int previousMaxActiveEnemies = -1;




    void Start()
    {
        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("EnemySpawner: Player with tag 'Player' not found!");

        // Initialize pools
        enemyPools = new List<List<GameObject>>();
        foreach (var prefab in enemyPrefabs)
        {
            List<GameObject> pool = new List<GameObject>(poolSizePerEnemy);
            for (int i = 0; i < poolSizePerEnemy; i++)
            { 
                GameObject enemy = Instantiate(prefab);
                enemy.SetActive(false);
                pool.Add(enemy);
            }
            enemyPools.Add(pool);
        }
    }


    void Update()
    {
        if (player == null)
            return;

        totalTime += Time.deltaTime;
        if (totalTime > 360f)
            return; // Stop spawning after 360 seconds

        int intervalIndex = Mathf.FloorToInt(totalTime / 45f);
        intervalIndex = Mathf.Clamp(intervalIndex, 0, maxEnemiesPerInterval.Count - 1);

        maxActiveEnemies = maxEnemiesPerInterval[intervalIndex];

        if (maxActiveEnemies != previousMaxActiveEnemies)
        {
            Debug.Log($"[EnemySpawner] Max Active Enemies updated to: {maxActiveEnemies} at time {totalTime:F2}s");
            previousMaxActiveEnemies = maxActiveEnemies;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnAccordingToTime();
        }
    }




    void SpawnAccordingToTime()
    {
        // Determine which enemy indices to spawn based on totalTime
        List<int> enemiesToSpawn = new List<int>();

        if (totalTime < 45f)
        {
            enemiesToSpawn.Add(0); // 1st enemy only
             
        }
        else if (totalTime < 90f)
        {
            enemiesToSpawn.Add(1); // 2nd enemy only
            
        }
        else if (totalTime < 135f)
        {
            enemiesToSpawn.Add(2); // 3rd enemy only
            
        }
        else if (totalTime < 180f)
        {
            enemiesToSpawn.Add(0);
            enemiesToSpawn.Add(1); // 1st and 2nd
            
        }
        else if (totalTime < 225f)
        {
            enemiesToSpawn.Add(1);
            enemiesToSpawn.Add(2); // 2nd and 3rd
            
        }
        else if (totalTime < 270f)
        {
            enemiesToSpawn.Add(0);
            enemiesToSpawn.Add(2); // 1st and 3rd
            
        }
        else if (totalTime < 360f)
        {
            enemiesToSpawn.Add(0);
            enemiesToSpawn.Add(1);
            enemiesToSpawn.Add(2); // all three
            
        }

        // Count total active enemies across all pools
        int activeCount = 0;
        foreach (var pool in enemyPools)
        {
            foreach (var enemy in pool)
            {
                if (enemy.activeInHierarchy)
                    activeCount++;
            }
        }

        if (activeCount >= maxActiveEnemies)
            return; // Too many active enemies, skip spawning

        // For each enemy type to spawn, try to spawn one enemy
        foreach (int enemyIndex in enemiesToSpawn)
        {
            TrySpawnEnemyFromPool(enemyIndex);
        }
    }

    void TrySpawnEnemyFromPool(int poolIndex)
    {
        if (poolIndex < 0 || poolIndex >= enemyPools.Count)
            return;

        var pool = enemyPools[poolIndex];

        // Find inactive enemy in pool
        GameObject enemyToSpawn = null;
        foreach (var enemy in pool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemyToSpawn = enemy;
                break;
            }
        }

        if (enemyToSpawn == null)
            return; // No available enemy in pool

        // Calculate random spawn position around player
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        spawnPos.y = 0f; // Adjust if needed

        enemyToSpawn.transform.position = spawnPos;
        enemyToSpawn.transform.rotation = Quaternion.identity;

        var enemyHealth = enemyToSpawn.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.currentHealth = enemyHealth.maxHealth;
        }

        enemyToSpawn.SetActive(true);

        // Optional: Reset enemy state if needed
        // var enemyScript = enemyToSpawn.GetComponent<EnemyMovement>();
        // if (enemyScript != null) enemyScript.ResetEnemy();
    }

    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}
