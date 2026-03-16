using UnityEngine;

public class SickleSkill : MonoBehaviour
{
    public GameObject sicklePrefab;

    public float cooldown = 1.2f;

    float lastUse;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > lastUse + cooldown)
        {
            lastUse = Time.time;
            ThrowSickle();
        }
    }

    void ThrowSickle()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = (mousePos - transform.position).normalized;

        GameObject sickle = Instantiate(
            sicklePrefab,
            transform.position,
            Quaternion.identity
        );

        SickleBoomerang boom = sickle.GetComponent<SickleBoomerang>();

        boom.Init(direction, transform);
    }
}