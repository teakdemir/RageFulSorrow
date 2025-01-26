using UnityEngine;

public class EnemyDoctor : MonoBehaviour
{
    [Header("Player Interaction")]
    public GameObject player;
    public float speed;
    public float chaseRange;
    public Animator animator;

    private Transform playerPos;
    private float distance;
    private bool isChasing = false;

    [Header("Health")]
    public float currentHealth = 100f;
    public float maxHealth = 100f;

    [Header("Combat")]
    public float pushForce = 4f;
    public float damageAmount = 10f;
    public float attackCooldown = 0.2f;
    private float nextAttackTime;

    [Header("Loot")]
    public GameObject heartPrefab; // Can prefab'ı (Heart16)

    private void OnStart()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        distance = Vector2.Distance(transform.position, playerPos.position);

        // Oyuncuyu takip etme mantığı
        if (distance <= chaseRange)
        {
            isChasing = true;
            animator.SetBool("IsWalking", true);

            transform.position = Vector2.MoveTowards(
                transform.position,
                playerPos.position,
                speed * Time.deltaTime
            );
        }
        else
        {
            isChasing = false;
            animator.SetBool("IsWalking", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // Oyuncuya hasar ver
                playerStats.TakeDamage(damageAmount);

                // Oyuncuyu geri it
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                    playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                }
            }

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        // Oyuncunun sorrow barını artırmak için (opsiyonel)
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.DealDamage();
        }

        // Canı sıfıra düşerse öl
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
       

        // Heart16 prefab'ını düşür
        if (heartPrefab != null)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }
       
        Destroy(gameObject);
    }
}