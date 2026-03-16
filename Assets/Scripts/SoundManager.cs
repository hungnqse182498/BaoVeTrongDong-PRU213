using UnityEngine;
using UnityEngine.UI; // Cần thư viện này để tương tác với giao diện (UI)

public class SoundManager : MonoBehaviour
{
    bool isMuted = false;

    [Header("UI Cài đặt")]
    public Image soundButtonImage; // Khung chứa hình ảnh của nút
    public Sprite soundOnSprite;   // Hình cái loa đang bật
    public Sprite soundOffSprite;  // Hình cái loa bị gạch chéo

    // Hàm này sẽ được gọi khi bạn bấm nút trên màn hình
    public void ToggleSound()
    {
        isMuted = !isMuted; // Đảo ngược trạng thái (đang bật thành tắt, tắt thành bật)

        if (isMuted)
        {
            // Tắt toàn bộ âm thanh của game
            AudioListener.volume = 0f;

            // Đổi hình nút thành loa gạch chéo
            if (soundButtonImage != null && soundOffSprite != null)
                soundButtonImage.sprite = soundOffSprite;
        }
        else
        {
            // Bật lại toàn bộ âm thanh (1f tương đương 100%)
            AudioListener.volume = 1f;

            // Đổi hình nút thành loa bình thường
            if (soundButtonImage != null && soundOnSprite != null)
                soundButtonImage.sprite = soundOnSprite;
        }
    }
}