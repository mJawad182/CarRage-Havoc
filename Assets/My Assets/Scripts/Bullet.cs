using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string targetTag; 
    public string ownTag;
    public float speed = 30f;
    public int damage = 10;
    public float lifeTime = 5f;  // Destroy bullet after this time if no hit

    void Start()
    {
        Destroy(gameObject, lifeTime);
       
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {

                enemyHealth.TakeDamage(damage);
            }

            Destroy(gameObject); // Destroy bullet on hit
        }
        else if (!other.CompareTag(ownTag) && !other.CompareTag("Bullet"))
        {
            // Optional: destroy bullet if it hits anything else except player or other bullets
            Destroy(gameObject);
        }
    }
}
