using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;

    // Oyuncu istatistikleri
    private PlayerStats playerStats;
    private PlayerController playerController;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Mermiyi ileri doğru yönünde hareket ettir
        rb.linearVelocity = transform.right * speed;

        // Sahnedeki PlayerStats referansını bul
        playerStats = FindObjectOfType<PlayerStats>();

        // Ateş animasyonu tetiklenmesi
        playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.TriggerShootAnimation();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("EnemyNurse"))
        {
            EnemyNurse nurse = collision.gameObject.GetComponent<EnemyNurse>();
            if (nurse != null)
            {
                nurse.TakeDamage(playerStats.GetCurrentDamage());
            }

            
            playerStats.DealDamage();
            Destroy(gameObject);
        }
       
        else if (collision.gameObject.CompareTag("EnemyDoctor"))
        {
            EnemyDoctor doc = collision.gameObject.GetComponent<EnemyDoctor>();
            if (doc != null)
            {
                doc.TakeDamage(playerStats.GetCurrentDamage());
            }

            playerStats.DealDamage();
            Destroy(gameObject);
        }
        
        else if (collision.gameObject.CompareTag("EnemyPatient"))
        {
            EnemyPatient patient = collision.gameObject.GetComponent<EnemyPatient>();
            if (patient != null)
            {
                patient.TakeDamage(playerStats.GetCurrentDamage());
            }

            playerStats.DealDamage();
            Destroy(gameObject);
        }
       
        else if (collision.gameObject.CompareTag("EnemyBoss"))
        {
            
            BossBehavior boss = collision.gameObject.GetComponent<BossBehavior>();
            if (boss != null)
            {
                boss.TakeDamage(playerStats.GetCurrentDamage());
            }

            
            playerStats.DealDamage();
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
