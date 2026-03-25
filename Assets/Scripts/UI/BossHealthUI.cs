using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    private Slider slider;
    private BossAI currentBoss;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetupBoss(BossAI boss)
    {
        currentBoss = boss;

        // Nếu slider vẫn null thì lấy ngay lúc này
        if (slider == null) slider = GetComponent<Slider>();

        slider.maxValue = boss.MaxHealth;
        slider.value = boss.MaxHealth;

        gameObject.SetActive(true); // Hiện thanh máu lên
    }

    void Update()
    {
        if (currentBoss == null) return;

        slider.value = currentBoss.CurrentHealth;

        if (currentBoss.isDead)
        {
            Invoke("HideBar", 2.5f); // Đợi diễn xong 22 frame chết
        }
    }

    void HideBar() => gameObject.SetActive(false);
}