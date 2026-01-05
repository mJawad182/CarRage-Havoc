using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float detectionRadius = 10f;   // How close the player must be to detect
    public float attackRange = 2f;
    public float moveSpeed = 5f;           // Enemy running speed
    public float rotationSpeed = 5f;       // How fast enemy rotates to face player
    public float attackBuffer = 0.5f;      // buffer to prevent flickering
    public float Stoprange = 1.5f;

    private Transform player;
    private bool playerDetected = false; 
    private bool isAttacking = false;

    public Animator animator;
    private NavMeshAgent agent;

    public float playerDamage;
    public int enemyDamage;

    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;// Cached reference to player health

    void Start()
    {
        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
            enemyHealth = transform.GetComponent<EnemyHealth>();
            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth component not found on Player!");
            }
        }
        else
        {
            Debug.LogError("Player with tag 'Player' not found!");
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing on enemy!");
            enabled = false;
            return;
        }

        // Set agent speed and stopping distance
        agent.speed = moveSpeed;
        agent.stoppingDistance = Stoprange;

        // Initialize animator parameters
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttack", false);
        animator.SetFloat("SpeedMultiplier", 1f);
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        playerDetected = distance <= detectionRadius;

        if (playerDetected)
        {
            if (!isAttacking)
            {
                // Chase player if outside attack range
                if (distance > attackRange)
                {
                    isAttacking = false;
                    agent.isStopped = false;
                    agent.SetDestination(player.position);

                    animator.SetBool("IsRunning", true);
                    animator.SetBool("IsAttack", false);
                    animator.SetFloat("SpeedMultiplier", 1f);
                }
                else
                {
                    // Enter attack mode
                    isAttacking = true;
                    agent.isStopped = true;

                    animator.SetBool("IsRunning", false);
                    animator.SetBool("IsAttack", true);
                    animator.SetFloat("SpeedMultiplier", 1.5f);
                }
            }
            else
            {

                // Stay in attack mode until player moves beyond attackRange + buffer
                if (distance > attackRange + attackBuffer)
                {
                    isAttacking = false;
                    agent.isStopped = false;
                    agent.SetDestination(player.position);

                    animator.SetBool("IsRunning", true);
                    animator.SetBool("IsAttack", false);
                    animator.SetFloat("SpeedMultiplier", 1f);
                }
                else
                {
                    // Keep attacking
                    agent.isStopped = true;

                    animator.SetBool("IsRunning", false);
                    animator.SetBool("IsAttack", true);
                    animator.SetFloat("SpeedMultiplier", 1.5f);
                }
            }

            // Smoothly blend upper body layer weight
            float targetWeight = isAttacking ? 0.5f : 0f;
            int upperBodyLayer = animator.GetLayerIndex("UpperBody");
            float currentWeight = animator.GetLayerWeight(upperBodyLayer);
            float newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * 10f);
            animator.SetLayerWeight(upperBodyLayer, newWeight);
        }
        else
        {
            // Player not detected: stop moving and reset animations
            agent.isStopped = true;

            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttack", false);

            float targetWeight = 0f;
            int upperBodyLayer = animator.GetLayerIndex("UpperBody");
            float currentWeight = animator.GetLayerWeight(upperBodyLayer);
            float newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * 10f);
            animator.SetLayerWeight(upperBodyLayer, newWeight);
        }
    }

    // Optional: visualize detection radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void ResetEnemy()
    {
        // Reset health, states, animations, etc.
        animator.Play("Idle");
        // Add your reset logic here
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(playerDamage);
                enemyHealth.TakeDamage(enemyDamage);
                Handheld.Vibrate();

            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(playerDamage);
                enemyHealth.TakeDamage(enemyDamage);
                Handheld.Vibrate();

            }
        }
    }
}
