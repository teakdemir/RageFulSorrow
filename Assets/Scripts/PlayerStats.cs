using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public float currentDamage = 20f;

    private Rigidbody2D rb;
    public Animator animator;
    private bool isPushed = false;

    void Start()
    {
        currentHealth = maxHealth;
        currentDamage = 20f;
        rb = GetComponent<Rigidbody2D>();
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
        currentDamage = 20f + (currentRage / 10) - (currentSorrow / 10);
        currentDamage = Mathf.Max(currentDamage, 1f); // Minimum damage is 1
    }

    public void TakeDamage(float damageAmount)
    {
        // Play damage sound effect
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.takeDamage);
        }

        animator.SetBool("IsDamaged", true);
        StartCoroutine(ResetDamageAnimation());
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        IncreaseRage(10f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(0.3f);  // Adjust this to match your animation length
        animator.SetBool("IsDamaged", false);
    }

    public void DealDamage()
    {
        // Decrease damage by 1 and increase sorrow by 10
        currentDamage = Mathf.Max(currentDamage - 1f, 1f); // Decrease damage by 1 but ensure it doesn't go below 1
        
        IncreaseSorrow(5f); // Increase sorrow by 10
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

    //player itildiğinde sonsuza kadar itilmesin diye bu ve altındaki
    public void Push(Vector2 pushDirection, float pushForce)
    {
        if (!isPushed)
        {
            StartCoroutine(PushCoroutine(pushDirection, pushForce));
        }
    }
    
    IEnumerator PushCoroutine(Vector2 pushDirection, float pushForce)
    {
        isPushed = true;
        rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        rb.linearVelocity = Vector2.zero;
        isPushed = false;
    }

    private bool isDead = false;

    void Die()
    {
        if (isDead) return; // Prevent multiple deaths

        // Play death sound effect
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.Death);
        }

        isDead = true;
        GameStateManager.Instance.FreezeGame();

        // Stop all movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true; // This prevents any physics forces from affecting the player
        }

        // Play death animation
        animator.SetBool("IsDead", true);

        StartCoroutine(HandleDeath());
    }
    
    public static class GameData
    {
        public static int LastPlayedLevel { get; set; }
    }

    private IEnumerator HandleDeath()
    {
        // Wait for animation to complete
        yield return new WaitForSeconds(2f); // Regular WaitForSeconds is fine now

        // Optional: Add any effects here before destroying

        // Destroy the player
        Destroy(gameObject);
        GameData.LastPlayedLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Retry");
    }


    public float GetCurrentDamage()
    {
        return currentDamage;
    }
}