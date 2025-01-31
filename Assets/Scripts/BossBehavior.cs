using System.Collections;
using UnityEngine;
using static PlayerStats;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class BossBehavior : MonoBehaviour
{
    [Header("Boss Stats")]
    public float maxHealth = 300f;
    private float currentHealth;
    public float collisionDamage = 20f;

    [SerializeField] BossHealthBar healthBar;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chargeSpeed = 8f;
    public float chargeDuration = 2f;

    [Header("Attack Settings")]
    public float chargeInterval = 10f;

    private Rigidbody2D rb;
    public Animator animator;

    private GameObject player;
    private bool isCharging = false;
    private Vector3 chargeTargetPosition;

    private BossGun bossGun;

    void Start()
    {
        // Boss'un gerekli bileşenlerini ayarla
        gameObject.tag = "EnemyBoss";
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        healthBar = GetComponentInChildren<BossHealthBar>();
        rb = GetComponent<Rigidbody2D>();
        
        player = GameObject.FindGameObjectWithTag("Player");

        // BossGun bileşenini bul
        bossGun = GetComponentInChildren<BossGun>();
        if (bossGun == null)
        {
            Debug.LogWarning("BossBehavior: BossGun scripti bulunamadı! Ateş etme çalışmaz.");
        }

        // Charge saldırı başlat
        StartCoroutine(ChargeRoutine());
    }

    void Update()
    {
        if (GameStateManager.Instance.IsGameFrozen)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        if (player == null) return;

        // Eğer charge yapmıyorsa normal hızda oyuncuyu takip et
        if (!isCharging)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    IEnumerator ChargeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(chargeInterval);

            if (player != null)
            {
                chargeTargetPosition = player.transform.position;
            }

            isCharging = true;
            float startTime = Time.time;
            Vector2 chargeDirection = (chargeTargetPosition - transform.position).normalized;

            while (Time.time < startTime + chargeDuration)
            {
                rb.linearVelocity = chargeDirection * chargeSpeed;
                yield return null;
            }

            isCharging = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (GameStateManager.Instance.IsGameFrozen)
            return;

        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private bool isDead = false;

    void Die()
    {
        if (isDead) return; // Prevent multiple deaths

        isDead = true;
        GameStateManager.Instance.FreezeGame();

        // Stop all movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true; // This prevents any physics forces from affecting the player
        }

        animator.SetBool("IsDead", true);

        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        // Wait for animation to complete
        yield return new WaitForSeconds(1.8f); // Regular WaitForSeconds is fine now

        // Optional: Add any effects here before destroying

        // Destroy the player
        Destroy(gameObject);
        SceneManager.LoadScene("Thanks");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameStateManager.Instance.IsGameFrozen)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(collisionDamage);

                // Oyuncuyu geri it
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerStats.Push(pushDirection, 5f);
            }
        }
    }
}
