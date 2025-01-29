using System.Collections;
using UnityEngine;

/* Bu arkadaş range'e girince patlıyor */
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
    private bool hasExploded = false; // Patlama durumu

    private Transform playerPos;
    private float distance;
    private bool isChasing = false;

    private Rigidbody2D rb;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameStateManager.Instance.IsGameFrozen)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
            animator.SetBool("IsWalking", false);
            return;
        }

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
            if (distance <= 2f && !hasExploded)
            {
                Die();
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (GameStateManager.Instance.IsGameFrozen)
            return;

        if (hasExploded) return; // Patladıysa daha fazla işlem yapma

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

    void Die()
    {
        animator.SetBool("IsDead", true);

        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        // Wait for animation to complete
        yield return new WaitForSeconds(0.8f); // Regular WaitForSeconds is fine now

        // Heart16 prefab'ını düşür
        if (heartPrefab != null)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the player
        Destroy(gameObject);
    }
}



