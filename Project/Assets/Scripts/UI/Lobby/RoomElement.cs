using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomElement : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text playerNumText;
    [SerializeField] private Button enterButton;

    public void SetView(string name, int currentNum, int maxNum)
    {
        nameText.text = name;
        playerNumText.text = $"{currentNum}/{maxNum}";
    }

    public void SetButtonEvent(string str)
    {
        enterButton.onClick.AddListener(() => OnEnterButtonClicked(str));
    }

    private void OnEnterButtonClicked(string str)
    {
        LobbyUIPresentor presentor = FindAnyObjectByType<LobbyUIPresentor>();
        presentor.ShowJoinPasswordWindow();
        PasswordWindow passwordWindow = FindAnyObjectByType<PasswordWindow>();
        passwordWindow.SetRoomName(str);
    }
}
