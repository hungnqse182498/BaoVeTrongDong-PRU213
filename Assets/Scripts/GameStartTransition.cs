using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStartTransition : MonoBehaviour
{
    public Image whiteScreen;       // Kéo ảnh WhiteFlash vào đây
    public float fadeSpeed = 1.5f;  // Thời gian mờ đi (giây)
    public Manager gameManager;     // Kéo object chứa Manager.cs vào đây

    void Start()
    {
        // Vừa vào game là bật màn hình trắng lên ngay
        whiteScreen.gameObject.SetActive(true);
        whiteScreen.color = new Color(1, 1, 1, 1);

        StartCoroutine(FadeOutWhite());
    }

    IEnumerator FadeOutWhite()
    {
        float timer = 0f;
        Color col = whiteScreen.color;

        while (timer < fadeSpeed)
        {
            timer += Time.deltaTime;
            col.a = Mathf.Lerp(1f, 0f, timer / fadeSpeed); // Làm mờ từ từ
            whiteScreen.color = col;
            yield return null;
        }

        whiteScreen.gameObject.SetActive(false);

        gameManager.StartGameLoop();
    }
}