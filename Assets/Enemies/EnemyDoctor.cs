using UnityEngine;
/*bu arkadasimiz yakin dovuscu*/

public class EnemyDoctor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject player;
    public float speed;
    public float chaseRange;
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
        }
    }
}
