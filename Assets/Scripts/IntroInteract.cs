using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Data.SqlTypes;
using System.Collections;

public class IntroInteract : MonoBehaviour
{
    public GameObject hintText;   // Kéo thả cái UI Text "[E] Chạm vào" vào đây
    public VideoPlayer myVideo;   // Kéo thả Video Player của bạn vào đây
    public AudioClip[] linhHonVoices;

    private bool canTouch = false;

    IEnumerator Start()
    {
        myVideo.loopPointReached += OnVideoEnd;
        hintText.SetActive(false);
        yield return new WaitForSecondsRealtime(3f);
        DialogueManager.Instance.ShowDialogue("Linh Hồn Trống Đồng", "Hãy chạm vào ta... thức tỉnh ký ức của ngươi.", linhHonVoices[0]);
        
    }

    void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsFinished())
        {
            if (canTouch && Input.GetKeyDown(KeyCode.R))
            {
                DialogueManager.Instance.CloseDialogue();
                hintText.SetActive(false);

                // TRƯỚC KHI PLAY VIDEO
                Time.timeScale = 0f; // Dừng game để xem video
                myVideo.Play();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hintText.SetActive(true);
            canTouch = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hintText.SetActive(false);
            canTouch = false;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Time.timeScale = 1f; // Đảm bảo game không bị đóng băng
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level");
    }
}