using System.Collections;
using UnityEngine;

public class BossBullet : MonoBehaviour 
{
    [Header("Damage & Lifetime")]
    public float damageAmount = 10f;
    public float lifeTime = 2f;
    public float speed = 5f;

    [Header("Split Settings")]
    public bool canSplit = true;
    public GameObject splitBulletPrefab;
    public int splitCount = 3;
    public float splitSpreadAngle = 45f;

    private Rigidbody2D rb;
    private GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (canSplit)
        {
            if (player != null)
            {
                Vector2 direction = ((Vector2)player.transform.position - rb.position).normalized;
                rb.linearVelocity = direction * speed;
            }
            StartCoroutine(SplitAfterTime(lifeTime));
        }
        else
        {
            if (player != null)
            {
                Vector2 direction = ((Vector2)player.transform.position - rb.position).normalized;
                rb.linearVelocity = direction * speed;
            }
        }
    }

    IEnumerator SplitAfterTime(float t)
    {
        yield return new WaitForSeconds(t);
        
        if (canSplit)
        {
            SplitIntoMultiple();
        }
        
        Destroy(gameObject);
    }

    void SplitIntoMultiple()
    {
        if (splitBulletPrefab == null || player == null) return;

        Vector2 directionToPlayer = ((Vector2)player.transform.position - rb.position).normalized;
        float angleStep = splitSpreadAngle / (splitCount - 1);
        float startAngle = -splitSpreadAngle / 2f;

        for (int i = 0; i < splitCount; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            Vector2 newDirection = RotateVector(directionToPlayer, currentAngle);
            
            GameObject newBullet = Instantiate(splitBulletPrefab, transform.position, Quaternion.identity);
            
            BossBullet bulletScript = newBullet.GetComponent<BossBullet>();
            if (bulletScript != null)
            {
                bulletScript.canSplit = false;
            }

            Rigidbody2D newRb = newBullet.GetComponent<Rigidbody2D>();
            if (newRb != null)
            {
                newRb.linearVelocity = newDirection * speed;
            }
        }
    }

    Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats ps = collision.gameObject.GetComponent<PlayerStats>();
            if (ps != null)
            {
                ps.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("EnemyBoss"))
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}