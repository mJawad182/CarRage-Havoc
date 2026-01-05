using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 90f; // degrees per second 

    [Header("Attraction")]
    public float attractionRadius = 5f; // radius to start moving toward player
    public float moveSpeed = 5f;        // speed of moving toward player

    private Transform player;
    private bool isAttracted = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player with tag 'Player' not found!");
    }

    void Update()
    {
        // Rotate around Y axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isAttracted && distance <= attractionRadius)
        {
            isAttracted = true;
        }

        if (isAttracted)
        {
            // Move toward player smoothly
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (UiManager.Instance != null)
            {
                UiManager.Instance.GainPoints(1);
                PowerPoints powerpoint = GameObject.Find("Power").GetComponent<PowerPoints>();
                powerpoint.PowerPoint();
            }
            else
            {
                Debug.LogError("UiManager instance not found!");
            }
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
