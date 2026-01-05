using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Animator animator;
    NavMeshAgent agent;
    
    public ParticleSystem destryEffect;
    EnemyMovement enemyMovement;
    public GameObject coinPrefeb;

    void Awake()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        enemyMovement = GetComponent<EnemyMovement>(); // Cache movement script
    }

    public void TakeDamage(int amount) 
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log("ddddd");
        
    }

    void Die()
    {
        

        // Disable movement/AI on death

        if (destryEffect != null)
        {
            ParticleSystem effectInstance = Instantiate(destryEffect, transform.position + Vector3.up*0.5f, Quaternion.identity);
            effectInstance.Play();
            Destroy(effectInstance.gameObject, effectInstance.main.duration + effectInstance.main.startLifetime.constantMax);
        }

        Vector3 coinSpawnPos = transform.position + Vector3.up * 0.2f; // raise coin 0.5 units above enemy position
        Instantiate(coinPrefeb, coinSpawnPos, Quaternion.identity);
        //Vector3 spawnPos = transform.position;
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5f))
        //{
        //    spawnPos.y = hit.point.y + 0.1f; // slightly above ground
        //}
        //Instantiate(coinPrefeb, spawnPos, Quaternion.identity);


        gameObject.SetActive(false);
        Debug.Log("Enemy killed and deactivated");
    }



    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void ResetEnemy()
    {
         

        ResetHealth();

    }


    

}
