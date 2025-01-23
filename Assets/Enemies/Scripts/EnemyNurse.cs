using UnityEngine;
/*bu arkadas uzak dovuscu bi seyler firlatiyor*/

public class EnemyNurse : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float chaseRange;
    public float attackRange;
    public Animator animator;

    private float distance;
    private bool isChasing = false;
    void Start()
    {
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= chaseRange)
        {
            if (distance > attackRange)
            {
                isChasing = true;
                animator.SetBool("IsWalking", true);
                // Chase the player
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    player.transform.position,
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
    }

    void Attack()
    {
        Debug.Log("Enemy is attacking!");
    }
}