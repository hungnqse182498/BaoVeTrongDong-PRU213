using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Cập nhật UI hiển thị số lượng trái cây trong túi đồ (PlayerBackpack) bằng tiếng Việt.
/// </summary>
public class BackpackUI : MonoBehaviour
{
    [Header("Cài đặt văn bản")]
    [SerializeField] string labelText = "Túi đồ"; // Thay thế cho localizableString

    [Header("Tham chiếu")]
    [SerializeField] PlayerBackpack backpack;

    Text text;

    void Start()
    {
        text = GetComponent<Text>();

        // Tự động tìm backpack nếu quên kéo vào Inspector
        if (backpack == null)
        {
            backpack = Object.FindFirstObjectByType<PlayerBackpack>();
        }
    }

    void Update()
    {
        if (backpack == null || text == null) return;

        // Hiển thị dạng: Túi đồ:
        //               5 / 10
        text.text = labelText + ":\n" + backpack.current + " / " + backpack.max;
    }
}