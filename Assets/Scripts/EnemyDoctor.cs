using UnityEngine;
/*bu arkadasimiz yakin dovuscu*/

public class EnemyDoctor : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float chaseRange;
    public Animator animator;

    private Transform playerPos;
    private float distance;
    private bool isChasing = false;
    
    public float currentHealth = 100f, maxHealth = 100f;
    public float pushForce = 4f;
    float damageAmount = 10f;
    float attackCooldown = 0.2f;
    private float nextAttackTime;
    

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        distance = Vector2.Distance(transform.position, playerPos.position);

        if (distance <= chaseRange)
        {
            isChasing = true;
            animator.SetBool("IsWalking", true);

            transform.position = Vector2.MoveTowards(
                transform.position,
                playerPos.position,
                speed * Time.deltaTime
            );
        }
        else
        {
            isChasing = false;
            animator.SetBool("IsWalking", false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);

                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerStats.Push(pushDirection, pushForce);
            }

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.DealDamage();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
