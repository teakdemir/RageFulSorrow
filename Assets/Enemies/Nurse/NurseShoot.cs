using UnityEngine;

public class NurseShoot : MonoBehaviour
{
    public GameObject scissors;
    public Transform scissorsPos;

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2)
        {
            timer = 0;
            shoot();
        }


    }

    void shoot()
    {
        Instantiate(scissors, scissorsPos.position, Quaternion.identity);
    }
}
