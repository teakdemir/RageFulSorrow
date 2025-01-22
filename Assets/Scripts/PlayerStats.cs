using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("UI References")]
    public Image SorrowBarFill;
    public Image RageBarFill;
    public Image HealthBarFill;

    [Header("Gameplay Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxSorrow = 100f;
    public float maxRage = 100f;
    public float currentSorrow = 0f;
    public float currentRage = 0f;
    public float currentDamage = 10f;

    void Start()
    {
        currentHealth = maxHealth;
        currentDamage = 10f;
    }

    void Update()
    {
        UpdateUI();
        UpdateDamage();
    }

    void UpdateUI()
    {
        SorrowBarFill.fillAmount = currentSorrow / maxSorrow;
        RageBarFill.fillAmount = currentRage / maxRage;
        HealthBarFill.fillAmount = currentHealth / maxHealth;
    }

    void UpdateDamage()
    {
        // Base damage is 10
        // Rage increases damage, Sorrow decreases it
        currentDamage = 10f + (currentRage / 10) - (currentSorrow / 10);
        currentDamage = Mathf.Max(currentDamage, 1f); // Minimum damage is 1
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        IncreaseRage(10f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void DealDamage()
    {
        // Decrease damage by 1 and increase sorrow by 10
        currentDamage = Mathf.Max(currentDamage - 1f, 1f); // Decrease damage by 1 but ensure it doesn't go below 1
        IncreaseSorrow(10f); // Increase sorrow by 10
    }

    public void IncreaseSorrow(float amount)
    {
        currentSorrow += amount;
        currentSorrow = Mathf.Clamp(currentSorrow, 0, maxSorrow);
        currentRage -= amount;
        currentRage = Mathf.Clamp(currentRage, 0, maxRage);

        if (currentSorrow >= maxSorrow)
        {
            Die();
        }
    }

    public void IncreaseRage(float amount)
    {
        currentRage += amount;
        currentRage = Mathf.Clamp(currentRage, 0, maxRage);
        currentSorrow -= amount;
        currentSorrow = Mathf.Clamp(currentSorrow, 0, maxSorrow);

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
