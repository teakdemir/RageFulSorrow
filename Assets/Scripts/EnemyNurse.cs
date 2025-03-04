using System.Collections;
using UnityEngine;

/* Bu arkadaş uzak dövüşçü, bir şeyler fırlatıyor */
public class EnemyNurse : MonoBehaviour
{
    [Header("Player Interaction")]
    public GameObject player;
    public float speed;
    public float chaseRange;
    public float attackRange;
    public Animator animator;

    [Header("Health")]
    public float currentHealth = 100f;
    public float maxHealth = 100f;

    [Header("Combat")]
    public float pushForce = 5f;
    public float damageAmount = 5f;
    public float collideCooldown = 0.3f;
    private float nextCollideTime;

    [Header("Shooting")]
    public GameObject scissorsPrefab;
    public Transform scissorsSpawnPos;
    public float shootCooldown = 2f; 
    private float shootTimer;

    [Header("Loot")]
    public GameObject heartPrefab; // Can prefab'ı (ör. Heart16)

    private Transform playerPos;
    private float distance;
    private bool isChasing = false;

    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        shootTimer = shootCooldown; // İlk atış için bekleme süresi dolmuş gibi başlat
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
        {
            if (distance > attackRange)
            {
                // Oyuncuya yaklaş
                isChasing = true;
                animator.SetBool("IsWalking", true);
                transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
            }
            else
            {
                // Oyuncu menzil içinde, saldırı başlasın
                isChasing = false;
                animator.SetBool("IsWalking", false);
                Attack();
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameStateManager.Instance.IsGameFrozen)
            return;

        if (collision.gameObject.CompareTag("Player") && Time.time >= nextCollideTime)
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // Oyuncuya hasar ver
                playerStats.TakeDamage(damageAmount);

                // Oyuncuyu geri it
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerStats.Push(pushDirection, pushForce);
            }

            nextCollideTime = Time.time + collideCooldown;
        }
    }

    void Attack()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootCooldown)
        {
            shootTimer = 0;
            Shoot();
        }
    }

    void Shoot()
    {
        // Play scissors sound effect when shooting
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.scissors);
        }

        if (scissorsPrefab != null && scissorsSpawnPos != null)
        {
            Instantiate(scissorsPrefab, scissorsSpawnPos.position, Quaternion.identity);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (GameStateManager.Instance.IsGameFrozen)
            return;

        currentHealth -= damageAmount;

        // Oyuncunun sorrow barını artır (isteğe bağlı)
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.DealDamage();
        }

        // Can 0'ın altına düştüyse öl
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Play nurse death sound
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.nursedeath);
        }

        animator.SetBool("IsDead", true);
        speed = 0; // **Stop movement**
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // **Ensure rigidbody stops moving**
        }
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        // Wait for animation to complete
        yield return new WaitForSeconds(0.8f);

        // Heart16 prefab'ını düşür
        if (heartPrefab != null)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the enemy
        Destroy(gameObject);
    }
}