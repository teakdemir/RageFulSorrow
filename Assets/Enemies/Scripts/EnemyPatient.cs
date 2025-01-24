using UnityEngine;
/*bu arkadas da range'e girince patliyor manyak bi sey*/
public class EnemyPatient : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float chaseRange;
    public Animator animator;

    public float explosionDamage = 20f;
    public float maxHealth = 50f;
    private float currentHealth;

    private Transform playerPos;
    private float distance;
    private bool isChasing = false;
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        distance = Vector2.Distance(transform.position, playerPos.position);

        if (distance <= chaseRange)
            isChasing = true;

        if (isChasing)
        {
            isChasing = true;
            animator.SetBool("IsWalking", true);
            transform.position = Vector2.MoveTowards(
                transform.position,
                playerPos.position,
                speed * Time.deltaTime
            );
            if (distance <= 1.5f) // Patlama mesafesi
            {
                Explode();
            }
        }
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        /*BU YORUM SATIRLARI MERGE EDÝNCE AÇILACAK
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.DealDamage();
        }
        */

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Explode()
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(explosionDamage);
        }
        Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}