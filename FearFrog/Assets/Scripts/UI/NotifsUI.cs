using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotifsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text notificationUI;

    public void ShowNotif(string keyName)
    {
        notificationUI.text = "Find " + keyName;
    }

    public void HideNotif()
    {
        notificationUI.text = string.Empty;
    }
}
