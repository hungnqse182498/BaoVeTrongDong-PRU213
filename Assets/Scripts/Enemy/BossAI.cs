using System.Collections;
using UnityEngine;

/// <summary>
/// Boss Logic: Điều khiển Boss với bộ Animation phức tạp (nhiều frame).
/// </summary>
public class BossAI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int attackDamage = 5;
    [SerializeField] float attackCooldown = 2.0f;

    [Header("Animation Timings (15 Frames Attack)")]
    [Tooltip("Thời gian từ lúc bắt đầu đánh đến khi gây sát thương (thường là frame thứ 10/15)")]
    [SerializeField] float damageFrameDelay = 1.0f;
    [Tooltip("Thời gian diễn nốt animation sau khi đã gây sát thương")]
    [SerializeField] float finishAnimDelay = 0.5f;

    [Header("Audio")]
    [SerializeField] AudioSource bossAudioSource;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip screamSound; // Tiếng gầm khi xuất hiện


    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool left;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isHurt;
    [HideInInspector] public bool isDead;

    private int currentHealth;
    private float attackTimer;
    private Artifact artifact;
    private bool bossActive = false;

    // Getters cho script UI hoặc HealthBar
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        GameObject artObj = GameObject.FindGameObjectWithTag("Artifact");
        if (artObj != null) artifact = artObj.GetComponent<Artifact>();
        
        BossHealthUI healthUI = GameObject.FindAnyObjectByType<BossHealthUI>(FindObjectsInactive.Include);
        if (healthUI != null)
        {
            healthUI.SetupBoss(this);
        }

        attackTimer = Time.time + attackCooldown;

        if (bossAudioSource != null && screamSound != null)
        {
            bossAudioSource.PlayOneShot(screamSound);
        }

        EnemyHealth eh = GetComponent<EnemyHealth>();
        if (eh != null)
        {
            eh.max = maxHealth;
            eh.current = currentHealth;
        }

        bossActive = true;
    }

    void Update()
    {
        if (isDead || !bossActive) return;

        // Nếu đang bị choáng/nhận sát thương thì không làm gì
        if (isHurt)
        {
            if (artifact != null)
                left = artifact.transform.position.x < transform.position.x;
            return;
        }

        HandleBossMovement();
    }

    void HandleBossMovement()
    {
        if (artifact == null) return;

        float distance = Vector2.Distance(transform.position, artifact.transform.position);

        // Khoảng cách dừng của Boss (thường to hơn quái thường vì Boss to)
        if (distance > 2.0f)
        {
            MoveTowards(artifact.transform.position);
        }
        else
        {
            isMoving = false;

            // Kiểm tra Cooldown và trạng thái đánh
            if (!isAttacking && Time.time > attackTimer)
            {
                StartCoroutine(AttackRoutine());
                attackTimer = Time.time + attackCooldown;
            }
        }

        // Quay mặt về hướng mục tiêu
        left = artifact.transform.position.x < transform.position.x;
    }

    void MoveTowards(Vector3 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        isMoving = true;
        isAttacking = false;
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 1. Chờ đến frame "vung tay" (ví dụ frame 10)
        yield return new WaitForSeconds(damageFrameDelay);

        // 2. Gây sát thương và phát âm thanh
        if (artifact != null)
        {
            artifact.Damage(attackDamage);
            if (bossAudioSource != null && attackSound != null)
            {
                bossAudioSource.PlayOneShot(attackSound);
            }
        }

        // 3. Chờ diễn nốt các frame thu tay về (ví dụ 5 frame cuối)
        yield return new WaitForSeconds(finishAnimDelay);

        isAttacking = false;
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        currentHealth = Mathf.Max(0, currentHealth);

        // Cập nhật UI máu nếu có
        EnemyHealth eh = GetComponent<EnemyHealth>();
        if (eh != null) eh.current = currentHealth;

        if (currentHealth > 0)
            StartCoroutine(HurtRoutine());
        else
            StartCoroutine(DieRoutine());
    }

    IEnumerator HurtRoutine()
    {
        isHurt = true;
        isMoving = false;
        // Boss có thể không bị ngắt chiêu khi trúng đòn (tùy bạn chọn)
        // isAttacking = false; 
        yield return new WaitForSeconds(0.4f); // Khớp với 5 frame hit
        isHurt = false;
    }

    IEnumerator DieRoutine()
    {
        isDead = true;
        isMoving = false;
        isAttacking = false;
        isHurt = false;

        Debug.Log("BOSS DEFEATED!");

        // Báo cho Manager biết Boss đã chết để thắng màn chơi
        // Dùng FindFirstObjectByType để thay thế (Chuẩn Unity mới)
        Manager mgr = Object.FindFirstObjectByType<Manager>();
        if (mgr != null) mgr.OnBossKilled();
        // Đợi diễn xong 22 frame chết (22 * 0.1s = 2.2s) rồi mới biến mất
        yield return new WaitForSeconds(2.2f);
        Destroy(gameObject);
    }
}