using UnityEngine;
/*bu arkadasimiz yakin dovuscu*/

public class EnemyDoctor : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float chaseRange;
    public Animator animator;

    private float distance;
    private bool isChasing = false;

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= chaseRange)
        {
            isChasing = true;
            animator.SetBool("IsWalking", true);

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
        }
    }
}
