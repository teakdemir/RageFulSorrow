using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    

    [Header("Game Play")]
    public float speed;
    private Vector2 movement;
    public int playerHealth = 3;
    void Start()
    {
        
    }

    void Update()
    {    

        FlipCharacter();


        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector2(
            movement.x * speed,
            movement.y * speed
        );

      
    }

  

 void FlipCharacter()
    {
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        
        Vector3 playerPosition = transform.position;

        
        if (mousePosition.x > playerPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); 
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); 
        }
    }


}
