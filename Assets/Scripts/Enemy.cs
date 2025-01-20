using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 3f;
    [SerializeField] float damageToPlayer = 1f;  // Düşmanın oyuncuya vereceği hasar
    private float maxHealth;

    private void Start()
    {
        maxHealth = health;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject); // Düşman öldüğünde yok olur
        }
    }

    // Düşmanla çarpışınca oyuncuya hasar ver
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Oyuncuya hasar ver
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageToPlayer); // Oyuncunun canını azalt
            }
        }
    }
}
