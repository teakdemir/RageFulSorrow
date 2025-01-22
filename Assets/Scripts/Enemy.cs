using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 100f;
    [SerializeField] float damageAmount = 10f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float pushForce = 10f;

    private float nextAttackTime;

    private void Start()
    {
        health = maxHealth;
        nextAttackTime = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);

                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                    playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                }

                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.DealDamage();
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
