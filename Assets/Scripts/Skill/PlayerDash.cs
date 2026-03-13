using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    Rigidbody2D rb;
    PlayerMovement movement;

    bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;

        // lấy hướng từ PlayerMovement
        Vector2 dashDir = movement.MoveDirection;

        if (dashDir == Vector2.zero)
            dashDir = Vector2.right;

        // tắt movement tạm thời
        movement.enabled = false;

        rb.linearVelocity = dashDir.normalized * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;

        // bật lại movement
        movement.enabled = true;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}