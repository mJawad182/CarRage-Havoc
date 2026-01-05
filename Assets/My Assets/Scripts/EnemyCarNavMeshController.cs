using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCarNavMeshController : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;

    [Header("NavMeshAgent Settings")]
    public float maxSpeed = 30f;          // Max speed in m/s
    public float acceleration = 300f;     // Acceleration of the agent
    public float angularSpeed = 360f;     // Turning speed in degrees per second
    public float stoppingDistance = 1f;   // Distance to stop before reaching the player

    private NavMeshAgent agent;

    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerHealth = playerObj.GetComponent<PlayerHealth>();
            playerHealth = playerObj.GetComponent<PlayerHealth>();
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

        // Let NavMeshAgent control position and rotation automatically
        agent.updatePosition = true;
        agent.updateRotation = true;

        // Assign player if not set
       
            player = GameObject.FindGameObjectWithTag("Player");
        

        // Configure NavMeshAgent parameters
        agent.speed = maxSpeed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;
        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        if (player == null) return;

        agent.SetDestination(player.transform.position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(0.1f);
                enemyHealth.TakeDamage(5);
                Handheld.Vibrate();

            }
            
        }
    }



}
