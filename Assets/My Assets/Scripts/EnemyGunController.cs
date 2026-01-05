using System.Collections.Generic;
using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Gun Settings")]
    public float detectionRadius = 50f;      // How far the gun can detect enemies
    public float fireRate = 1f;              // Shots per second
    public GameObject bulletPrefab;          // Bullet prefab to instantiate
    public Transform firePoint;              // Where bullets spawn from
    public float bulletSpeed = 30f;
    public float bulletInterval = 1f;        // Time between shots (seconds)

    [Header("Effects")]
    public ParticleSystem effectPrefab;      // Muzzle flash or shooting effect prefab

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private GameObject currentTarget;
    private float fireTimer = 0f;
    private void Start()
    {
        currentTarget = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        UpdateEnemiesInRange();

        

        if (currentTarget != null)
        {
            AimAtTarget(currentTarget);

            fireTimer += Time.deltaTime;
            if (fireTimer >= bulletInterval / fireRate)
            {
                fireTimer = 0f;
                Shoot();
            }
        }
    }

    // Find all enemies within detection radius
    void UpdateEnemiesInRange()
    {
        enemiesInRange.Clear();

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                enemiesInRange.Add(hit.gameObject);
            }
        }
    }

    // Pick the nearest enemy as target
    
    // Smoothly rotate gun to face target
    void AimAtTarget(GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        if (direction == Vector3.zero)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    // Instantiate bullet and shoot towards target
    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || currentTarget == null)
            return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Play muzzle flash or shooting effect
        SpawnEffect(firePoint.position);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }

        // Optional: Destroy bullet after 5 seconds to avoid clutter
        Destroy(bullet, 5f);
    }


    // Instantiate and play shooting effect, then destroy it after duration
    public void SpawnEffect(Vector3 pos)
    {
        if (effectPrefab == null)
            return;

        effectPrefab.Play();
    }

    // Visualize detection radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
