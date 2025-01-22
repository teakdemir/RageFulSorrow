using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] float damageAmount = 1f;  // Düşman hasar miktarı
    [SerializeField] float collisionRadius = 0.5f; // Çarpma radius'u

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        // Eğer player ile çarpıştıysa hasar ver
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, collisionRadius, LayerMask.GetMask("Player"));
        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerStats>().TakeDamage(damageAmount);
            playerCollider.GetComponent<PlayerStats>().IncreaseRage(10); // Rage arttıkça damage artacak
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
