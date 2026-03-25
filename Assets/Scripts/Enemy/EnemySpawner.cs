using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quản lý việc sinh quái theo đợt (Wave) được gọi từ Manager.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] Transform wolfPrefab;
    [SerializeField] Transform wolfEaterPrefab;
    [SerializeField] Transform enemy01Prefab;
    [SerializeField] Transform enemy02Prefab;
    [SerializeField] Transform enemy03Prefab;
    [SerializeField] Transform bossPrefab; // Kéo Prefab Boss vào đây

    [Header("Spawn Settings")]
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] int eaterChance = 3;
    [SerializeField] float spawnTime = 1f;
    [SerializeField] float spawnReductionPer = 0.05f;
    [SerializeField] float spawnFloor = 0.2f;

    [Header("Boss Cutscene Settings")]
    [SerializeField] BossCutscene bossCutscene; // Kéo script quản lý video vào đây

    float currentSpawnTime;

    void Start()
    {
        currentSpawnTime = spawnTime;
    }

    /// <summary>
    /// Gọi các đợt quái thường (1, 2, 3, 4)
    /// </summary>
    public void SpawnWave(int enemiesToSpawn)
    {
        StartCoroutine(SpawnRoutine(enemiesToSpawn));
    }

    /// <summary>
    /// Gọi Boss (Đợt 5)
    /// </summary>
    public void SpawnBoss()
    {
        if (bossCutscene != null)
        {
            // Gọi Video trước, trong hàm này mình sẽ truyền logic đẻ boss vào sau
            StartCoroutine(ExecuteBossSpawnWithVideo());
        }
        else
        {
            // Nếu không có video thì đẻ boss như cũ (phòng hờ lỗi)
            JustSpawnBoss();
        }
    }

    // Coroutine đẻ quái rải rác theo thời gian
    IEnumerator SpawnRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnSingleEnemy();

            yield return null;
        }
    }

    IEnumerator ExecuteBossSpawnWithVideo()
    {
        // 1. Chạy Video Boss
        // (Lưu ý: Hàm PlayBossIntro trong script BossCutscene phải được thiết lập dừng game)
        bossCutscene.PlayBossIntro();

        // 2. Đợi cho đến khi Video chạy xong (Dựa vào độ dài video)
        // Chúng ta đợi bằng Realtime vì game đang bị pause (Time.timeScale = 0)
        yield return new WaitForSecondsRealtime((float)bossCutscene.bossVideo.length);

        // 3. Sau khi video kết thúc, mới đẻ Boss và đệ
        JustSpawnBoss();
    }

    void JustSpawnBoss()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        StartCoroutine(SpawnRoutine(4)); // Đẻ thêm đệ
    }

    // Logic random quái gốc của bạn
    void SpawnSingleEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 1. Kiểm tra tỉ lệ ra nhóm "Ăn bụi cây" (WolfEater và Enemy03)
        if (Random.Range(0, 11) <= eaterChance)
        {
            if (Random.Range(0, 2) == 0)
            {
                Instantiate(wolfEaterPrefab, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Instantiate(enemy03Prefab, spawnPoint.position, Quaternion.identity);
            }
        }
        else
        {
            // 2. Nếu không, chọn ngẫu nhiên 1 trong 3 loại tấn công trụ
            int randAttack = Random.Range(0, 3);

            if (randAttack == 0)
            {
                Instantiate(wolfPrefab, spawnPoint.position, Quaternion.identity);
            }
            else if (randAttack == 1)
            {
                Instantiate(enemy01Prefab, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Instantiate(enemy02Prefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }
}