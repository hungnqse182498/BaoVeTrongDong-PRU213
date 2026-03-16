using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioSource playerAudioSource; // Cái loa của Player
    [SerializeField] AudioClip dashSound;           // Đĩa nhạc tiếng lướt

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
            // Phát âm thanh lướt mà không làm ngắt các âm thanh khác đang kêu
            if (playerAudioSource != null && dashSound != null)
            {
                playerAudioSource.PlayOneShot(dashSound);
            }
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