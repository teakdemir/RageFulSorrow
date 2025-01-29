using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Game Play")]
    public float speed;
    private Vector2 movement;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        FlipCharacter();
        HandleMovement();
    }

    void HandleMovement()
    {
        if (GameStateManager.Instance.IsGameFrozen)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true; // Add this line
            return;
        }

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        rb.linearVelocity = new Vector2(
            movement.x * speed,
            movement.y * speed
        );

        bool isMoving = Mathf.Abs(movement.x) > 0.01f || Mathf.Abs(movement.y) > 0.01f;
        animator.SetBool("IsWalking", isMoving);
    }

    public void TriggerShootAnimation()
    {
        animator.SetBool("IsShooting", true);
        StartCoroutine(ResetShootAnimation());

    }

    private IEnumerator ResetShootAnimation()
    {
        yield return new WaitForSeconds(0.3f);  // Adjust this to match your animation length
        animator.SetBool("IsShooting", false);
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