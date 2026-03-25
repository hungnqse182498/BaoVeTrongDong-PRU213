using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    [Header("UI Kết Nối")]
    public TextMeshProUGUI storyText;    // Kéo Text vào
    public Image storyArtDisplay;       // Kéo Image hiện ảnh vào
    public AudioSource voiceSource;     // Kéo AudioSource phát giọng vào

    [Header("Dữ Liệu Cốt Truyện")]
    [TextArea(3, 10)]
    public string[] paragraphs;         // Nội dung chữ
    public Sprite[] sprites;            // Ảnh minh họa tương ứng
    public AudioClip[] voices;          // Voice thoại tương ứng

    [Header("Cấu Hình")]
    public float typeSpeed = 0.05f;     // Tốc độ hiện chữ
    public string nextSceneName;        // Tên Scene game chính để chuyển vào

    private bool isTyping = false;
    private bool cancelTyping = false;
    private bool moveToNext = false;

    void Start()
    {
        StartCoroutine(PlayStory());
    }

    void Update()
    {
        // Nhận diện Click chuột hoặc phím Space để tua/qua câu
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                cancelTyping = true; // Nếu đang chạy chữ -> Tua nhanh
            }
            else
            {
                moveToNext = true;   // Nếu chữ xong rồi -> Qua câu tiếp
            }
        }
    }

    IEnumerator PlayStory()
    {
        for (int i = 0; i < paragraphs.Length; i++)
        {
            // Reset trạng thái cho câu mới
            moveToNext = false;
            cancelTyping = false;

            if (i < sprites.Length) storyArtDisplay.sprite = sprites[i];

            // 2. Chạy voice
            if (i < voices.Length && voices[i] != null)
            {
                voiceSource.Stop();
                voiceSource.clip = voices[i];
                voiceSource.Play();
            }

            // 3. Hiệu ứng đánh máy
            storyText.text = "";
            isTyping = true;
            foreach (char c in paragraphs[i].ToCharArray())
            {
                if (cancelTyping) // Nếu người dùng Click khi đang chạy chữ
                {
                    storyText.text = paragraphs[i]; // Hiện full câu ngay lập tức
                    break;
                }
                storyText.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }
            isTyping = false;

            yield return new WaitUntil(() => moveToNext);
            voiceSource.Stop(); // Dừng voice khi qua câu mới
        }

        // Hết truyện thì vào game
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

    // Hàm này để gọi khi nhấn nút
    public void SkipStory()
    {
        // Dừng tất cả các Coroutine đang chạy chữ/ảnh để tránh lỗi
        StopAllCoroutines();

        // Chuyển thẳng đến Scene game chính (nhớ gõ đúng tên Scene của bạn)
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}