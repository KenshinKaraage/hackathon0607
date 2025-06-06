using Photon.Pun;
using TMPro;
using UnityEngine;

public class PasswordWindow : MonoBehaviour
{
    [SerializeField] private TMP_InputField passInputField;
    private string roomName;

    public void SetRoomName(string name)
    {
        roomName = name;
    }
    
    public void OnClickSubmit()
    {
        string password = passInputField.text.Trim();

        if (!string.IsNullOrEmpty(password))
        {
            RoomManager.Instance.password = password;
            RoomTableManager roomTableManager = FindAnyObjectByType<RoomTableManager>();
            RoomManager.Instance.JoinRoom(roomName, password);
        }
        else
        {
            Debug.LogWarning("パスワードが未入力です。");
        }
    }

    public void OnClickBack()
    {
        LobbyUIPresentor presentor = FindAnyObjectByType<LobbyUIPresentor>();
        presentor.ShowRoomTable();
    }
}
