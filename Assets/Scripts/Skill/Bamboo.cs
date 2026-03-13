using UnityEngine;

public class Bamboo : MonoBehaviour
{
    public float lifeTime = 1.2f;
    public int damage = 10;

    public float growSpeed = 8f;

    Vector3 targetScale;

    void Start()
    {
        targetScale = transform.localScale;

        // bắt đầu từ dưới đất
        transform.localScale = new Vector3(targetScale.x, 0, targetScale.z);

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // mọc lên dần
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * growSpeed
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
            }
        }
    }
}