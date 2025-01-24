using UnityEngine;
/*bu arkadas uzak dovuscu bi seyler firlatiyor*/

public class EnemyNurse : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float chaseRange;
    public float attackRange;
    public Animator animator;

    public float currentHealth = 100f, maxHealth = 100f;
    public float pushForce = 5f;
    float damageAmount = 5f;
    float collideCooldown = 0.3f;
    private float nextCollideTime;

    private Transform playerPos;
    private float distance;
    private bool isChasing = false;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        distance = Vector2.Distance(transform.position, playerPos.position);

        if (distance <= chaseRange)
        {
            if (distance > attackRange)
            {
                isChasing = true;
                animator.SetBool("IsWalking", true);
                // Chase the player
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
                // Attack repeatedly by calling in Update
                Attack();
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextCollideTime)
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);

                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerStats.Push(pushDirection, pushForce);
            }

            nextCollideTime = Time.time + collideCooldown;
        }
    }

    void Attack()
    {
        Debug.Log("Enemy is attacking!");
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        /* BU YORUM SATIRLARI MERGE EDÝNCE AÇILACAK
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.DealDamage();
        }*/

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