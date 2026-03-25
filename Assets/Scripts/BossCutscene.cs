using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class BossCutscene : MonoBehaviour
{
    public VideoPlayer bossVideo;

    public void PlayBossIntro()
    {
        // 1. Tạm dừng game để xem phim
        Time.timeScale = 0f;

        // 2. Chạy Video (Nó sẽ tự hiện lên Camera Near Plane)
        if (bossVideo != null)
        {
            bossVideo.Play();
            // Đợi hết video rồi quay lại game
            StartCoroutine(WaitAndEndVideo());
        }
    }

    IEnumerator WaitAndEndVideo()
    {
        // Đợi bằng Realtime vì TimeScale = 0
        yield return new WaitForSecondsRealtime((float)bossVideo.length);

        // Sau khi xong, game chạy tiếp, Video sẽ tự biến mất (vì hết clip)
        Time.timeScale = 1f;

        // Bạn có thể dùng lệnh này để chắc chắn nó biến mất ngay lập tức
        bossVideo.Stop();
    }
}