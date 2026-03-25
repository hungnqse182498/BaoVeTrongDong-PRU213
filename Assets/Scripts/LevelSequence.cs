using System.Collections;
using UnityEngine;

public class LevelSequence : MonoBehaviour
{
    public AudioClip[] linhHonVoices; 
    public AudioClip hoVeVoice;

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(3f);

        DialogueManager.Instance.ShowDialogue("Linh Hồn Trống Đồng", "Hàng trăm năm qua... các người chỉ canh giữ một cái vỏ bọc. Cuối cùng... cũng có kẻ chạm được vào linh hồn ta.", linhHonVoices[0]);
        yield return new WaitUntil(() => DialogueManager.Instance.IsFinished());

        DialogueManager.Instance.ShowDialogue("Linh Hồn Trống Đồng", "Ta là linh hồn của Trống Đồng, một thực thể tồn tại giữa ký ức và hiện thực.", linhHonVoices[1]);
        yield return new WaitUntil(() => DialogueManager.Instance.IsFinished());
        
        DialogueManager.Instance.ShowDialogue("Linh Hồn Trống Đồng", "Ta đã chứng kiến sự thịnh vượng và suy tàn của làng, lưu giữ những ký ức quý giá về tổ tiên và lịch sử.", linhHonVoices[2]);
        yield return new WaitUntil(() => DialogueManager.Instance.IsFinished());

        DialogueManager.Instance.ShowDialogue("Linh Hồn Trống Đồng", "Nhưng giờ đây, một mối đe dọa mới đã xuất hiện - những linh hồn bị lãng quên, những ký ức bị bỏ rơi đang trỗi dậy dưới hình dạng những sinh vật bóng tối gọi là Hắc Linh.", linhHonVoices[3]);
        yield return new WaitUntil(() => DialogueManager.Instance.IsFinished());

        DialogueManager.Instance.ShowDialogue("Linh Hồn Trống Đồng", "Hắc Linh không phải quỷ dữ hay sinh vật ngoài hành tinh. Chúng là những ký ức bị biến dạng, sinh ra từ sự oán hận, bị phản bội, bị lãng quên, không được ghi nhận", linhHonVoices[4]);
        yield return new WaitUntil(() => DialogueManager.Instance.IsFinished());

        DialogueManager.Instance.ShowDialogue("Linh Hồn Trống Đồng", "Chúng có khả năng xâm nhập vào tâm trí các thực thể khác, điều khiển chúng tấn công Trống Đồng.", linhHonVoices[5]);
        yield return new WaitUntil(() => DialogueManager.Instance.IsFinished());

        DialogueManager.Instance.ShowDialogue("Linh Hồn Trống Đồng", "Nếu không được ngăn chặn, chúng sẽ xóa sổ ký ức của ta, và khi đó... ta sẽ biến mất mãi mãi.", linhHonVoices[6]);
        yield return new WaitUntil(() => DialogueManager.Instance.IsFinished());

        DialogueManager.Instance.ShowDialogue("Linh Hồn Trống Đồng", "Hộ vệ, người được chọn hãy chiến đấu để trì hoãn sự lãng quên, gom nhặt lại những Trái cây ký ức (Tinh Quả) từ những Bụi cây đời thường quanh làng để vá lại linh hồn Trống Đồng.", linhHonVoices[7]);
        yield return new WaitUntil(() => DialogueManager.Instance.IsFinished());

        DialogueManager.Instance.ShowDialogue("Hộ Vệ", "Đừng lo, ta sẽ bảo vệ nơi này!", hoVeVoice);
        yield return new WaitUntil(() => DialogueManager.Instance.IsFinished());

        DialogueManager.Instance.ShowDialogue("Linh Hồn Trống Đồng", "Hãy cẩn trọng, sức mạnh của chúng rất đáng gờm.", linhHonVoices[8]);
        yield return new WaitUntil(() => DialogueManager.Instance.IsFinished());

    }
}