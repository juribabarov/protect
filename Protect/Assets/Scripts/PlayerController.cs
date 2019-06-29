using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character Attributes")]
    public float BASE_MOVEMENT_SPEED = 1;

    [Space]
    [Header("Character Statistics")]
    public Vector2 movementDirection;
    public float movementSpeed;

    [Space]
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    private void Update()
    {
        ControllInput();
        Move();
        Animate();
    }

    private void ControllInput()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0, 1);
        movementDirection.Normalize();
    }

    private void Move()
    {
        rb.velocity = movementDirection * movementSpeed * BASE_MOVEMENT_SPEED;
    }

    private void Animate()
    {
        if (movementDirection != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);
        }
        animator.SetFloat("Speed", movementSpeed);
    }
}
