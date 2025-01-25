using UnityEngine;

/* Bu arkadaş da range'e girince patlıyor, manyak bir şey */
public class EnemyPatient : MonoBehaviour
{
    [Header("Player Interaction")]
    private PlayerStats playerStats;
    public GameObject player;
    public float speed;
    public float chaseRange;
    public Animator animator;

    [Header("Health & Explosion")]
    public float explosionDamage = 20f;
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("Loot")]
    public GameObject heartPrefab; // Can prefab'ı (ör. Heart16)

    private Transform playerPos;
    private float distance;
    private bool isChasing = false;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        distance = Vector2.Distance(transform.position, playerPos.position);

        if (distance <= chaseRange)
            isChasing = true;

        if (isChasing)
        {
            animator.SetBool("IsWalking", true);
            transform.position = Vector2.MoveTowards(
                transform.position,
                playerPos.position,
                speed * Time.deltaTime
            );

            // Patlama mesafesi
            if (distance <= 2f)
            {
                Explode();
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (playerStats != null)
        {
            playerStats.DealDamage();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Explode()
    {
        if (playerStats != null)
        {
            // Oyuncuya patlama hasarı ver
            playerStats.TakeDamage(explosionDamage);
        }

        Die();
    }

    void Die()
    {
        Debug.Log("EnemyPatient died. Dropping heart...");

        // Heart16 prefab'ını düşür
        if (heartPrefab != null)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Heart prefab is not assigned in the Inspector!");
        }

        Destroy(gameObject);
    }
}

