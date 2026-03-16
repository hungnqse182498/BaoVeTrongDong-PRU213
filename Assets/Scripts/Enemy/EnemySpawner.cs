using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component responsible of spawning wolves, given set respawn points and spawn frequency values.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform wolfPrefab;
    [SerializeField] Transform wolfEaterPrefab;
    [SerializeField] Transform enemy01Prefab;
    [SerializeField] Transform enemy02Prefab;
    [SerializeField] Transform enemy03Prefab;

    [SerializeField] Transform[] spawnPoints;

    [SerializeField] int eaterChance = 3;     //Chance out of 10 wolves to spawn an eater wolf
    [SerializeField] float spawnTime;         //Initial spawn delay per wolf
    [SerializeField] float spawnReductionPer; //Reduction in spawn delay per each wolf spawn
    [SerializeField] float spawnFloor;        //Minimum spawn delay per wolf

    float currentSpawnTime;
    float timer;

    void Start()
    {
        currentSpawnTime = spawnTime;
        timer = Time.time;
    }

    void Update()
    {
        if(Time.time > timer)
        {
            Spawn();
            currentSpawnTime -= spawnReductionPer;
            if(currentSpawnTime <= spawnFloor)
            {
                currentSpawnTime = spawnFloor;
            }
            timer = Time.time + currentSpawnTime;
        }
    }
    //void Spawn()
    //{
    //    //Calculate eater wolf chance
    //    if(Random.Range(0,11) > eaterChance)
    //    {
    //        Instantiate(wolfPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
    //    }
    //    else
    //    {
    //        Instantiate(wolfEaterPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
    //    }
    //}

    void Spawn()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 1. Kiểm tra tỉ lệ ra nhóm "Ăn bụi cây" (WolfEater và Enemy03)
        if (Random.Range(0, 11) <= eaterChance)
        {
            // Random 50/50 để chọn 1 trong 2 loại ăn bụi
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
            int randAttack = Random.Range(0, 3); // Trả về 0, 1 hoặc 2

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
