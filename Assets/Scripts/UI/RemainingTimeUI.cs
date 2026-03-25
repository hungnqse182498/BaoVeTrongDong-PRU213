using UnityEngine;
using UnityEngine.UI;

public class RemainingTimeUI : MonoBehaviour
{
    [Header("Cấu hình chữ")]
    public string prefixText = "Thời gian tới đợt";
    public string suffixText = "giây";
    public string bossText = "ĐỢT CUỐI: BOSS XUẤT HIỆN!";

    public Manager manager;
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        if (manager == null) manager = Object.FindAnyObjectByType<Manager>();
    }

    void Update()
    {
        if (manager == null || text == null) return;

        // Lấy thời gian còn lại
        float timeVal = manager.GetTimeUntilNextWave();
        int displayTime = Mathf.Max(0, (int)timeVal);

        // Kiểm tra xem có đang ở đợt cuối (Boss) không
        if (manager.currentWave >= manager.waveTimings.Length)
        {
            text.text = bossText;
        }
        else
        {
            // Hiển thị dạng: Thời gian tới đợt 2: 15 giây
            // (manager.currentWave + 1) vì trong code wave bắt đầu từ 0
            int nextWaveNumber = manager.currentWave + 1;
            text.text = prefixText + " " + nextWaveNumber + ": " + displayTime + " " + suffixText;
        }
    }
}