using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public AudioSource voiceSource;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI contentText;

    [Header("Settings")]
    public float typeSpeed = 0.04f;

    private bool isTyping = false;
    private string fullMessage; // Lưu nội dung đầy đủ để hiện ngay khi click nhanh

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }
    private bool readyToNext = false; // Biến để báo cho LevelSequence biết là đi tiếp được rồi
    public void ShowDialogue(string name, string message, AudioClip voiceClip = null)
    {
        readyToNext = true;
        dialoguePanel.SetActive(true);
        Time.timeScale = 0f;
        if (voiceClip != null && voiceSource != null)
        {
            voiceSource.Stop(); 
            voiceSource.PlayOneShot(voiceClip);     
        }
        nameText.text = name;
        fullMessage = message; // Lưu lại lời thoại
        StopAllCoroutines();
        StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string message)
    {
        isTyping = true;
        contentText.text = "";
        foreach (char c in message.ToCharArray())
        {
            contentText.text += c;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        isTyping = false;
    }

    // --- HÀM MỚI: GẮN VÀO NÚT BẤM HOẶC CLICK MÀN HÌNH ---
    public void OnClickDialogue()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            contentText.text = fullMessage;
            isTyping = false;
        }
        else
        {
            readyToNext = true;

            if (voiceSource.isPlaying) voiceSource.Stop();
            CloseDialogue();
        }
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game khi đóng thoại
    }

    // Sửa lại IsFinished để check cả việc Panel đã tắt hay chưa
    public bool IsFinished()
    {
        // Chỉ xong khi chữ chạy xong VÀ Voice phát xong
        bool voiceDone = (voiceSource == null || !voiceSource.isPlaying);
        return !isTyping && voiceDone && !dialoguePanel.activeSelf;
    }
}