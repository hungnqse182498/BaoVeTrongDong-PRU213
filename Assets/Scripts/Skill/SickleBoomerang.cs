using UnityEngine;

public class SickleBoomerang : MonoBehaviour
{
    public float speed = 12f;
    public float maxDistance = 6f;
    public float rotateSpeed = 720f;
    public int damage = 20;

    private Transform player;
    private Vector3 direction;
    private Vector3 startPos;

    private bool returning = false;

    public void Init(Vector3 dir, Transform owner)
    {
        direction = dir;
        player = owner;
        startPos = transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        if (!returning)
        {
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(startPos, transform.position) > maxDistance)
            {
                returning = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, player.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();

            if (enemy != null)
                enemy.DamageEnemy(damage);
        }
    }
}