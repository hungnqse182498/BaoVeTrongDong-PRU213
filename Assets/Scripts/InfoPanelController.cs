using UnityEngine;

public class InfoPanelController : MonoBehaviour
{
    public GameObject[] panels; // tất cả panel

    public void ShowPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panels[index].SetActive(true);
    }
}