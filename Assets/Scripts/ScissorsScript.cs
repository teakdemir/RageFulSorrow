using UnityEngine;

public class ScissorsScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    public float damageAmount = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Oyuncuya hasar ver
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerStats.Push(pushDirection, force);
            }

            // Makas� yok et
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("EnemyDoctor"))
        {
            Destroy(gameObject);
        }  else if (!collision.gameObject.CompareTag("EnemyNurse"))
        {
            Destroy(gameObject);
        }  else if (!collision.gameObject.CompareTag("EnemyPatient"))
        {
            Destroy(gameObject);
        }
       
    }

    private void OnBecameInvisible()
    {
        // Makas g�r�nmez oldu�unda yok et
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
