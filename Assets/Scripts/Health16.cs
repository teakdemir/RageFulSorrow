using UnityEngine;

public class Heart16 : MonoBehaviour
{
    [Header("Health Settings")]
    public float healthIncreaseAmount = 20f; // Ne kadar can artıracağı

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Oyuncu ile çarpışma kontrolü
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // Oyuncunun canını artır
                playerStats.currentHealth += healthIncreaseAmount;
                playerStats.currentHealth = Mathf.Clamp(playerStats.currentHealth, 0, playerStats.maxHealth);

                // Konsolda bilgi göster (isteğe bağlı)
                Debug.Log("Player healed! Current Health: " + playerStats.currentHealth);

                // Prefab'i yok et
                Destroy(gameObject);
            }
        }
    }
}
