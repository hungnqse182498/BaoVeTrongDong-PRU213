using UnityEngine;

public class BambooSkill : MonoBehaviour
{
    public GameObject bambooPrefab;

    [SerializeField] int rows = 12;
    [SerializeField] float rowDistance = 7f;
    [SerializeField] float spreadAngle = 90f;

    public float skillCooldown = 1.5f;

    float lastUseTime;

    [SerializeField] AudioSource skillAudioSource;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time > lastUseTime + skillCooldown)
        {
            lastUseTime = Time.time;
            SpawnBamboo();
            if (skillAudioSource != null)
            {
                skillAudioSource.Play();
            }
        }
    }

    void SpawnBamboo()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = (mousePos - transform.position).normalized;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        for (int r = 1; r <= rows; r++)
        {
            int bambooCount = r * 2;

            float angleStep = spreadAngle / Mathf.Max(1, bambooCount - 1);

            for (int i = 0; i < bambooCount; i++)
            {
                float angle = baseAngle - spreadAngle / 2 + angleStep * i;

                Vector3 dir = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad),
                    0
                );

                Vector3 spawnPos = transform.position + dir * (rowDistance * r);

                // Spawn bamboo
                GameObject bamboo = Instantiate(bambooPrefab, spawnPos, Quaternion.identity);

                // Random scale cho tự nhiên
                float randomScale = Random.Range(0.9f, 1.2f);
                bamboo.transform.localScale *= randomScale;
            }
        }
    }
}