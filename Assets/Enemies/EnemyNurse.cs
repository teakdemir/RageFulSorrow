using UnityEngine;
/*bu arkadas uzak dovuscu bi seyler firlatiyor*/

public class EnemyNurse : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float chaseRange;
    public float attackRange;

    private float distance;
    private bool isChasing = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= chaseRange)
            isChasing = true;

        if (isChasing)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                speed * Time.deltaTime
            );

            /*if(distance <= attackRange)
            {
                isChasing=false;
                Attack();
            }*/
        }
    }
    void Attack()
    {
        Debug.Log("Enemy is attacking");
    }
}