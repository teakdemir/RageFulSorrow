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
    public float explosionRange = 2f; 
    public float maxHealth = 50f;
    public float currentHealth=50f;

    [Header("Loot")]
    public GameObject heartPrefab; 
    private bool hasExploded = false; 

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

            
            if (distance <= explosionRange && !hasExploded)
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
        if (GameStateManager.Instance.IsGameFrozen || hasExploded)
            return;

        currentHealth -= damageAmount;

        if (playerStats != null)
        {
            playerStats.DealDamage();
        }

        if (currentHealth <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return; 

        hasExploded = true; 
        animator.SetBool("IsDead", true);
        speed = 0; 

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; 
        }
        GetComponent<Collider2D>().enabled = false;

        
        if (playerStats != null && distance <= explosionRange)
        {
            playerStats.TakeDamage(explosionDamage);
        }

        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        
        yield return new WaitForSeconds(0.8f);

        // Heart16 prefab'ını düşür
        if (heartPrefab != null)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }

       
        Destroy(gameObject);
    }
}
