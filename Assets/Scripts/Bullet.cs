using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;

    // PlayerStats referansı
    private PlayerStats playerStats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    [System.Obsolete]
    private void Start()
    {
        // Mermiye doğru yönünde hız ver
        rb.linearVelocity = transform.right * speed;

        // Oyuncu istatistiklerini bul
        playerStats = FindObjectOfType<PlayerStats>();
    }

    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            // PlayerStats içindeki currentDamage'i kullanarak hasar ver
            if (playerStats != null)
            {
                enemyComponent.TakeDamage(playerStats.GetCurrentDamage());
                
            }

           
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("Player"))
        {
            
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
       
        Destroy(gameObject);
    }
}



