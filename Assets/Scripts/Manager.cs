using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game state manager: Quản lý đợt quái theo thời gian thực và kiểm tra điều kiện thắng (Giết Boss).
/// </summary>
public class Manager : MonoBehaviour
{
    [Header("References")]
    public Artifact artifact;
    public EnemySpawner spawner; // Kéo Enemy Spawner vào đây
    public DialogueManager dialogueManager; // Kéo DialogueManager (Linh Hồn hoặc Hộ Vệ) vào đây
    SceneManager sceneManager;

    [Header("Wave Timings (Seconds)")]
    // Thời gian gọi quái (tính từ lúc game bắt đầu)
    public float[] waveTimings = { 10f, 60f, 90f, 120f, 150f };

    public int currentWave = 0;
    public bool isGameActive = false;

    private float gameTimer = 0f;

    [Header("Boss Tracking")]
    public int bossAlive = 0;
    private bool bossHasSpawned = false;
    private bool isEnding = false; // Biến cờ ngăn việc gọi Win/Lose nhiều lần

    public AudioClip[] linhHonVoices;
    public AudioClip hoVeVoices;

    void Awake()
    {
        sceneManager = GetComponent<SceneManager>();
        isGameActive = false;
        gameTimer = 0f;
        currentWave = 0;
        bossAlive = 0;
        bossHasSpawned = false;
    }

    void Update()
    {
        if (!isGameActive) return;

        if (artifact.health <= 0)
        {
            StartCoroutine(LoseSequence());
            return;
        }

        gameTimer += Time.deltaTime;

        if (currentWave < waveTimings.Length)
        {
            if (gameTimer >= waveTimings[currentWave])
            {
                StartWave(currentWave);
                currentWave++;
            }
        }
    }

    void StartWave(int waveIndex)
    {
        int waveNumber = waveIndex + 1;

        // Nếu là đợt cuối cùng -> Gọi Boss
        if (waveNumber == waveTimings.Length)
        {
            Debug.Log($"[{gameTimer:F1}s] Đợt {waveNumber} - BOSS XUẤT HIỆN!");
            if (spawner != null) spawner.SpawnBoss();

            bossAlive = 1;
            bossHasSpawned = true;
        }
        else
        {
            Debug.Log($"[{gameTimer:F1}s] Đợt {waveNumber} bắt đầu!");

            // Công thức tính số quái mỗi đợt (Đợt 1: 5 con, Đợt 2: 10 con...)
            int enemiesToSpawn = waveNumber * 5;
            if (spawner != null) spawner.SpawnWave(enemiesToSpawn);
        }
    }

    public void OnBossKilled()
    {
        bossAlive--;

        if (bossHasSpawned && bossAlive <= 0 && !isEnding)
        {
            StartCoroutine(WinSequence());
        }
    }
    IEnumerator WinSequence()
    {
        isEnding = true;
        isGameActive = false; 

        yield return new WaitForSeconds(4f);

        Debug.Log("BOSS CHẾT - ĐANG DỌN QUÁI");
        KillAllRemainingEnemies();

        yield return new WaitForSeconds(2f);

        dialogueManager.ShowDialogue("Linh Hồn Trống Đồng", "Bóng tối đã tan... Những tiếng vang nghìn năm cuối cùng đã tìm lại được nhịp đập. Ngươi đã hồi sinh ký ức của cả một dân tộc, người hộ vệ trẻ.", linhHonVoices[0]);
        yield return new WaitUntil(() => dialogueManager.IsFinished());

        dialogueManager.ShowDialogue("Hộ Vệ", "Tiếng Trống Đồng là hơi thở của đất mẹ. Chừng nào tôi còn đứng đây, không một mảnh ký ức nào bị lãng quên.", hoVeVoices);
        yield return new WaitUntil(() => dialogueManager.IsFinished());

        sceneManager.ChangeScene(7);
    }

    IEnumerator LoseSequence()
    {
        isEnding = true;
        isGameActive = false;

        yield return new WaitForSeconds(3f);
        Time.timeScale = 0f;  

        Debug.Log("TRỤ NÁT - ĐANG CHẠY HỘI THOẠI THUA");

        dialogueManager.ShowDialogue("Linh Hồn Trống Đồng", "Không... Trống Đồng đã vỡ... ta sẽ chìm trong lãng quên mãi mãi...", linhHonVoices[1]);
        yield return new WaitUntil(() => dialogueManager.IsFinished());

        Time.timeScale = 1f; 
        sceneManager.ChangeScene(3);
    }

    // --- DỌN SẠCH QUÁI TRÊN SÂN ---
    void KillAllRemainingEnemies()
    {
        // Tìm tất cả quái vật trên sân
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Thử tìm script EnemyAI (Quái thường)
            EnemyAI normalEnemy = enemy.GetComponent<EnemyAI>();
            if (normalEnemy != null)
            {
                normalEnemy.TakeDamage(9999); // Ép chết để chạy Animation
                continue;
            }

            // Thử tìm script BossAI (Nếu còn Boss nào khác)
            BossAI bossScript = enemy.GetComponent<BossAI>();
            if (bossScript != null)
            {
                bossScript.TakeDamage(9999);
            }
        }
    }

    // Thêm hàm này để script UI có thể lấy thời gian đếm ngược
    public float GetTimeUntilNextWave()
    {
        if (currentWave < waveTimings.Length)
        {
            return waveTimings[currentWave] - gameTimer;
        }
        return 0f;
    } 

    public void StartGameLoop()
    {
        isGameActive = true;
        gameTimer = 0f;
        Debug.Log("Sự lãng quên bắt đầu! Trận chiến đếm ngược...");
    }
}