using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float damageAmount = 10f;
    public float speed = 5f;    // Mermi hızı

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Mermiyi ileri yönünde hareket ettir
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameStateManager.Instance.IsGameFrozen)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats ps = collision.gameObject.GetComponent<PlayerStats>();
            if (ps != null)
            {
                ps.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("EnemyBoss") && !collision.gameObject.CompareTag("BossBullet"))
        {
            // Boss’a veya başka BossBullet’lara çarpınca yok olmasın
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        // Mermi ekrandan çıkınca yok et
        Destroy(gameObject);
    }
}
