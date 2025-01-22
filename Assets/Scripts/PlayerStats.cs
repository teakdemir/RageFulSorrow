using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("UI References")]
    public Image SorrowBarFill;  // Sorrow bar dolum kısmı
    public Image RageBarFill;    // Rage bar dolum kısmı
    public Image HealthBarFill;  // Health bar dolum kısmı

    [Header("Gameplay Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxSorrow = 100f;
    public float maxRage = 100f;
    public float currentSorrow = 0f;
    public float currentRage = 0f;
    public float baseDamage = 10f;
    private float currentDamage;

    void Start()
    {
        currentHealth = maxHealth;
        currentDamage = baseDamage;
    }

    void Update()
    {
        // Barların dolum oranlarını güncelle
        SorrowBarFill.fillAmount = currentSorrow / maxSorrow;
        RageBarFill.fillAmount = currentRage / maxRage;
        HealthBarFill.fillAmount = currentHealth / maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void IncreaseSorrow(float amount)
    {
        currentSorrow += amount;
        currentSorrow = Mathf.Clamp(currentSorrow, 0, maxSorrow);

        // Sorrow arttıkça Rage azalacak
        currentRage -= amount;
        currentRage = Mathf.Clamp(currentRage, 0, maxRage);

        currentDamage = baseDamage * (1f - (currentSorrow / maxSorrow));

        if (currentSorrow >= maxSorrow)
        {
            Die();
        }
    }

    public void IncreaseRage(float amount)
    {
        currentRage += amount;
        currentRage = Mathf.Clamp(currentRage, 0, maxRage);

        // Rage arttıkça Sorrow azalacak
        currentSorrow -= amount;
        currentSorrow = Mathf.Clamp(currentSorrow, 0, maxSorrow);

        currentDamage = baseDamage * (1f + (currentRage / maxRage));

        if (currentRage >= maxRage)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }

    public float GetCurrentDamage()
    {
        return currentDamage;
    }
}
