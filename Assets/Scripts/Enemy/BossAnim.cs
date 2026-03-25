using System.Collections;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
    [Header("Animation Sprites")]
    [SerializeField] private Sprite[] idleSprites;   // 6 frames
    [SerializeField] private Sprite[] walkSprites;   // 12 frames
    [SerializeField] private Sprite[] attackSprites; // 15 frames
    [SerializeField] private Sprite[] hurtSprites;   // 5 frames
    [SerializeField] private Sprite[] deathSprites;  // 22 frames

    [Header("Settings")]
    [SerializeField] private float frameTime = 0.1f; // Boss nên để 0.1s cho mượt vì nhiều frame

    private SpriteRenderer spriteRenderer;
    private BossAI bossAI;
    private int frameIndex = 0;
    private float timer;
    private bool isDead = false;

    private Sprite[] lastAnim = null;
    private bool isPlayingOnce = false;
    private Coroutine playOnceCoroutine = null;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossAI = GetComponent<BossAI>();
    }

    void Update()
    {
        if (isDead) return;

        Sprite[] currentAnim = GetCurrentAnim();
        if (currentAnim == null || currentAnim.Length == 0) return;

        // Nếu chuyển trạng thái animation
        if (currentAnim != lastAnim)
        {
            lastAnim = currentAnim;
            frameIndex = 0;
            timer = Time.time + frameTime;
            spriteRenderer.sprite = currentAnim[frameIndex];
        }

        // Chạy loop animation (Idle, Walk)
        if (!isPlayingOnce)
        {
            if (Time.time >= timer)
            {
                frameIndex = (frameIndex + 1) % currentAnim.Length;
                spriteRenderer.sprite = currentAnim[frameIndex];
                timer = Time.time + frameTime;
            }
        }

        spriteRenderer.flipX = bossAI.left;
    }

    private Sprite[] GetCurrentAnim()
    {
        if (bossAI.isDead)
        {
            if (playOnceCoroutine == null)
                playOnceCoroutine = StartCoroutine(PlayOnce(deathSprites, true));
            return deathSprites;
        }

        if (bossAI.isHurt)
        {
            if (playOnceCoroutine == null)
                playOnceCoroutine = StartCoroutine(PlayOnce(hurtSprites, false));
            return hurtSprites;
        }

        if (bossAI.isAttacking)
        {
            // Với Boss có 15 frame attack, ta nên dùng PlayOnce để diễn hết bộ chiêu
            if (playOnceCoroutine == null)
                playOnceCoroutine = StartCoroutine(PlayOnce(attackSprites, false, true));
            return attackSprites;
        }

        if (bossAI.isMoving)
            return walkSprites;

        return idleSprites;
    }

    // PlayOnce nâng cấp cho Boss
    private IEnumerator PlayOnce(Sprite[] anim, bool dieAfter, bool isAttack = false)
    {
        if (anim == null || anim.Length == 0) yield break;

        isPlayingOnce = true;
        for (int i = 0; i < anim.Length; i++)
        {
            spriteRenderer.sprite = anim[i];
            yield return new WaitForSeconds(frameTime);
        }

        isPlayingOnce = false;
        playOnceCoroutine = null;

        if (dieAfter)
        {
            isDead = true;
            // Giữ lại frame cuối của cảnh chết
            spriteRenderer.sprite = anim[anim.Length - 1];
        }
    }
}