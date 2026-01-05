using UnityEngine;
using System.Collections.Generic;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public float detectionRadius = 20f;      // How far the gun can detect enemies
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

    void Update()
    {
        UpdateEnemiesInRange();

        if (currentTarget == null || !enemiesInRange.Contains(currentTarget))
        {
            PickNearestTarget();
        }

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
    void PickNearestTarget()
    {
        if (enemiesInRange.Count == 0)
        {
            currentTarget = null;
            return;
        }

        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        Vector3 pos = transform.position;

        foreach (var enemy in enemiesInRange)
        {
            float dist = Vector3.Distance(pos, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        currentTarget = nearest; 
    }

    // Smoothly rotate gun to face target
    void AimAtTarget(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0f; // 🔹 Ignore vertical difference (keeps gun level)

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
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
