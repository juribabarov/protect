using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character Attributes")]
    public float BASE_MOVEMENT_SPEED = 2;
    public float BASE_ARROW_SPEED = 7;
    public float ARROW_DISTANCE = 1;
    public float BASE_AIMING_PENALTY = 0.7f;
    public float SHOOTING_RECOIL_TIME = 1;

    [Space]
    [Header("Character Statistics")]
    public Vector2 movementDirection;
    public float movementSpeed;
    public bool endOfAiming;
    public bool isAiming;
    public float shootingRecoil = 0;
    public bool useController;

    [Space]
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject directionArrow;

    [Space]
    [Header("Prefabs")]
    [SerializeField] private GameObject arrowPrefab;

    private void Update()
    {
        ControllInput();
        Move();
        Animate();
        Aim();
        Shoot();
    }

    private void ControllInput()
    {
        if (useController)
        {
            movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0, 1);
            movementDirection.Normalize();

            endOfAiming = Input.GetButtonUp("Fire1");
            isAiming = Input.GetButton("Fire1");
        }
        else
        {
            Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            movementDirection += mouseMovement;
            movementDirection.Normalize();

            movementSpeed = Input.GetAxis("Vertical");
            endOfAiming = Input.GetButtonUp("Fire1");
            isAiming = Input.GetButton("Fire1");
        }

        if (isAiming)
            movementSpeed *= BASE_AIMING_PENALTY;

        if (endOfAiming)
        {
            shootingRecoil = SHOOTING_RECOIL_TIME;
        }
        if (shootingRecoil > 0)
            shootingRecoil -= Time.deltaTime;
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

    private void Aim()
    {
        if (movementDirection != Vector2.zero)
        {
            directionArrow.transform.localPosition = movementDirection * ARROW_DISTANCE;

            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            directionArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void Shoot()
    {
        Vector2 shootingDirection = directionArrow.transform.localPosition;
        shootingDirection.Normalize();

        if (endOfAiming)
        {
            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            arrow.GetComponent<Rigidbody2D>().velocity = shootingDirection * BASE_ARROW_SPEED;
            arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
            Destroy(arrow, 2);
        }
    }
}
